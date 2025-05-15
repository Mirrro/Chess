using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Execution;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.MoveGeneration.Generators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.AI
{
    /// <summary>
    /// Implements a basic chess AI using the Minimax algorithm with alpha-beta pruning to find the best move.
    /// </summary>
    public class ChessAi
    {
        private readonly ExecutionService executionService;
        private readonly BurstMoveFinder movesFinder;
        private readonly TranspositionTable transpositionTable = new TranspositionTable();
        private readonly int[,] historyHeuristic = new int[64, 64];

        public ChessAi(ExecutionService executionService, BurstMoveFinder movesFinder)
        {
            this.executionService = executionService;
            this.movesFinder = movesFinder;
        }

        public async UniTask<IGameplayMove> FindBestMove(GameplayStateModel state, bool isWhite, int depth)
        {
            var bestMoves = new List<IGameplayMove>();
            await UniTask.RunOnThreadPool(() =>
            {
                var bestScore = int.MinValue;
                var possibleMoves = movesFinder.RunJob(state, isWhite, true);
                // Sorting
                possibleMoves = SortMoves(possibleMoves);
                foreach (var move in possibleMoves)
                {
                    executionService.ExecuteAI(state, move, null);
                    state.TurnCount++;
                    var score = MinimaxSync(state, depth - 1, int.MinValue, int.MaxValue, !isWhite, isWhite);

                    executionService.UndoAI(null);
                    state.TurnCount--;

                    if (score == bestScore)
                    {
                        bestMoves.Add(move);
                    }
                    else if (score > bestScore)
                    {
                        bestScore = score;
                        bestMoves.Clear();
                        bestMoves.Add(move);
                    }
                }
            });
            return bestMoves[Random.Range(0, bestMoves.Count)];
        }

        private List<IGameplayMove> SortMoves(List<IGameplayMove> moves)
        {
            var captures = new List<IGameplayMove>();
            var quiets = new List<IGameplayMove>();
            foreach (var move in moves)
            {
                if (IsKillerMove(move))
                    captures.Add(move);
                else
                    quiets.Add(move);
            }

            quiets = quiets.OrderByDescending(move =>
            {
                int from = GetSquareIndex(move.From);
                int to = GetSquareIndex(move.TargetPosition);
                return historyHeuristic[from, to];
            }).ToList();
            return captures.Concat(quiets).ToList();
        }
        
        private int GetSquareIndex(Vector2Int position) => position.y * 8 + position.x;

        private int MinimaxSync(GameplayStateModel state, int depth, int alpha, int beta, bool isMaximizing,
            bool isWhiteRoot)
        {
            ulong hash = ZobristHasher.ComputeHashForState(state);
                if (transpositionTable.TryGet(hash, out var entry) && entry.Depth >= depth)
                {
                    switch (entry.Type)
                    {
                        case EntryType.Exact:
                            return entry.Score;
                            break;
                        case EntryType.LowerBound:
                            alpha = Math.Max(alpha, entry.Score);
                            break;
                        case EntryType.UpperBound:
                            beta = Math.Min(beta, entry.Score);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (alpha >= beta)
                    {
                        return entry.Score;
                    }
                }

                if (depth == 0 || BoardEvaluator.IsGameOver(state))
                {
                    var eval = BoardEvaluator.EvaluateState(state);
                    int evalScore = isWhiteRoot ? eval.ScoreWhite - eval.ScoreBlack : eval.ScoreBlack - eval.ScoreWhite;
                
                    transpositionTable.Store(hash, new TranspositionTableEntry()
                    {
                        Score = evalScore,
                        Depth = depth,
                        Type = EntryType.Exact
                    });
                
                    return evalScore;
                }
                
                var possibleMoves = movesFinder.RunJob(state, isMaximizing, true);
                possibleMoves = SortMoves(possibleMoves);
                var bestValue = isMaximizing == isWhiteRoot ? int.MinValue : int.MaxValue;
                foreach (var possibleMove in possibleMoves)
                {
                    executionService.ExecuteAI(state, possibleMove, null);
                    state.TurnCount++;
                    var value = MinimaxSync(state, depth - 1, alpha, beta, !isMaximizing, isWhiteRoot);
                    executionService.UndoAI(null);
                    state.TurnCount--;

                    if (isMaximizing == isWhiteRoot)
                    {
                        if (value > bestValue)
                        {
                            bestValue = value;
                        }

                        alpha = Math.Max(alpha, value);
                    }
                    else
                    {
                        if (value < bestValue)
                        {
                            bestValue = value;
                        }

                        beta = Math.Min(beta, value);
                    }

                    if (beta <= alpha)
                    {
                        if (!IsKillerMove(possibleMove))
                        {
                            int from = GetSquareIndex(possibleMove.From);
                            int to = GetSquareIndex(possibleMove.TargetPosition);
                            historyHeuristic[from, to] += depth * depth;
                        }
                        break;
                    }
                    
                }

                EntryType entryType;
                if (bestValue <= alpha)
                    entryType = EntryType.UpperBound;
                else if (bestValue >= beta)
                    entryType = EntryType.LowerBound;
                else
                    entryType = EntryType.Exact;
                transpositionTable.Store(hash, new TranspositionTableEntry
                {
                    Score = bestValue,
                    Depth = depth,
                    Type = entryType
                });
                return bestValue;
        }

        private bool IsKillerMove(IGameplayMove move)
        {
            return move.GetSteps().Any(step => step is CapturePieceStep or PromotePieceStep or PlayerPromotionStep);
        }
    }
}
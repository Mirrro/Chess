using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Execution;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.MoveGeneration;
using Random = UnityEngine.Random;

namespace Gameplay.AI
{
    /// <summary>
    /// Implements a basic chess AI using the Minimax algorithm with alpha-beta pruning to find the best move.
    /// </summary>
    public class ChessAi
    {
        private readonly ExecutionService executionService;

        public ChessAi(ExecutionService executionService)
        {
            this.executionService = executionService;
        }

        public async UniTask<IGameplayMove> FindBestMove(GameplayStateModel state, bool isWhite, int depth)
        {
            var bestScore = int.MinValue;
            var bestMoves = new List<IGameplayMove>();

            var possibleMoves = GameplayMovesGenerator.GetMoves(state, isWhite, true);

            foreach (var move in possibleMoves)
            {
                var clone = state.Clone();
                executionService.ExecuteAI(clone, move, null);

                var score = await UniTask.RunOnThreadPool(() =>
                    MinimaxSync(clone, depth - 1, int.MinValue, int.MaxValue, !isWhite, isWhite));

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

            return bestMoves[Random.Range(0, bestMoves.Count)];
        }

        private int MinimaxSync(GameplayStateModel state, int depth, int alpha, int beta, bool isMaximizing,
            bool isWhiteRoot)
        {
            if (depth == 0 || BoardEvaluator.IsGameOver(state))
            {
                var eval = BoardEvaluator.EvaluateState(state);
                return isWhiteRoot ? eval.ScoreWhite - eval.ScoreBlack : eval.ScoreBlack - eval.ScoreWhite;
            }

            var possibleMoves = GameplayMovesGenerator.GetMoves(state, isMaximizing, true);

            var bestValue = isMaximizing == isWhiteRoot ? int.MinValue : int.MaxValue;

            foreach (var possibleMove in possibleMoves)
            {
                var clone = state.Clone();
                executionService.ExecuteAI(clone, possibleMove, null);
                var value = MinimaxSync(clone, depth - 1, alpha, beta, !isMaximizing, isWhiteRoot);

                if (isMaximizing == isWhiteRoot)
                {
                    bestValue = Math.Max(bestValue, value);
                    beta = Math.Max(beta, value);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                else
                {
                    bestValue = Math.Min(bestValue, value);
                    alpha = Math.Min(alpha, value);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }

            return bestValue;
        }
    }
}
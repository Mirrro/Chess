using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.MoveGeneration.Utility;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.MoveGeneration.Generators
{
    /// <summary>
    /// Generates legal gameplay moves for pawns, including promotions, captures, and en passant.
    /// </summary>
    public static class PawnGameplayMoveGenerator
    {
        public static List<IGameplayMove> GeneratePawnMoves(GameplayStateModel gameplayStateModel, int pawnId,
            bool isAi)
        {
            List<IGameplayMove> moves = new();

            if (!gameplayStateModel.TryGetPieceModelById(pawnId, out var pawnPieceModel))
            {
                return moves;
            }

            var direction = pawnPieceModel.IsColor ? 1 : -1;
            var promotionRank = pawnPieceModel.IsColor ? 7 : 0;

            // Move forward
            var moveTarget = pawnPieceModel.Position + new Vector2Int(0, direction);
            if (MoveGenerationHelper.IsInBounds(moveTarget) &&
                !MoveGenerationHelper.IsTileOccupied(gameplayStateModel, moveTarget))
            {
                if (moveTarget.y == promotionRank)
                {
                    AddPromotionMoves(moves, pawnId, pawnPieceModel.Position, moveTarget,
                        new List<IGameplayStep> {new MovePieceStep(pawnId, moveTarget)}, isAi);
                }
                else
                {
                    AddSimpleMove(moves, pawnId, pawnPieceModel.Position, moveTarget);
                }

                // Double move (only possible if single move is clear)
                if (!pawnPieceModel.HasMoved)
                {
                    var doubleMoveTarget = pawnPieceModel.Position + new Vector2Int(0, 2 * direction);
                    var enPassantSquare = pawnPieceModel.Position + new Vector2Int(0, direction);

                    if (MoveGenerationHelper.IsInBounds(doubleMoveTarget) &&
                        !MoveGenerationHelper.IsTileOccupied(gameplayStateModel, doubleMoveTarget))
                    {
                        AddDoubleMove(moves, pawnId, pawnPieceModel.Position, doubleMoveTarget, enPassantSquare);
                    }
                }
            }

            // Captures (Left & Right)
            foreach (var dx in new[] {-1, 1})
            {
                var captureTarget = pawnPieceModel.Position + new Vector2Int(dx, direction);
                if (!MoveGenerationHelper.IsInBounds(captureTarget))
                {
                    continue;
                }

                var targetPiece = gameplayStateModel.PieceMap.Values.FirstOrDefault(p =>
                    p.Position == captureTarget && p.IsColor != pawnPieceModel.IsColor);

                if (targetPiece == null)
                {
                    continue;
                }

                var baseSteps = new List<IGameplayStep>
                {
                    new MovePieceStep(pawnId, captureTarget),
                    new CapturePieceStep(pawnId, targetPiece.Id)
                };

                if (captureTarget.y == promotionRank)
                {
                    AddPromotionMoves(moves, pawnId, pawnPieceModel.Position, captureTarget, baseSteps, isAi);
                }
                else
                {
                    moves.Add(new GameplayMove(pawnPieceModel.Position, captureTarget, baseSteps));
                }
            }

            // En passant
            if (gameplayStateModel.TurnCount == gameplayStateModel.EnPassantTurn + 1 &&
                Vector2Int.Distance(pawnPieceModel.Position, gameplayStateModel.EnPassantTrapPosition) <= 1.5f)
            {
                var enPassantSteps = new List<IGameplayStep>
                {
                    new MovePieceStep(pawnId, gameplayStateModel.EnPassantTrapPosition),
                    new CapturePieceStep(pawnId, gameplayStateModel.EnPassantPieceId)
                };

                moves.Add(new GameplayMove(pawnPieceModel.Position, gameplayStateModel.EnPassantTrapPosition, enPassantSteps));
            }

            return moves;
        }

        private static void AddSimpleMove(List<IGameplayMove> moves, int pawnId, Vector2Int fromPosition,
            Vector2Int targetPosition)
        {
            moves.Add(new GameplayMove(fromPosition, targetPosition, new List<IGameplayStep>
            {
                new MovePieceStep(pawnId, targetPosition)
            }));
        }

        private static void AddDoubleMove(List<IGameplayMove> moves, int pawnId, Vector2Int fromPosition,
            Vector2Int targetPosition, Vector2Int enPassantSquare)
        {
            moves.Add(new GameplayMove(fromPosition, targetPosition, new List<IGameplayStep>
            {
                new MovePieceStep(pawnId, targetPosition),
                new SetEnPassantTrapStep(enPassantSquare, pawnId)
            }));
        }

        private static void AddPromotionMoves(List<IGameplayMove> moves, int pawnId, Vector2Int fromPosition, Vector2Int targetPosition,
            List<IGameplayStep> baseSteps, bool isAi)
        {
            if (isAi)
            {
                var rookSteps = new List<IGameplayStep>(baseSteps)
                {
                    new PromotePieceStep(pawnId, PieceType.Rook)
                };
                moves.Add(new GameplayMove(fromPosition, targetPosition, rookSteps));

                var bishopStep = new List<IGameplayStep>(baseSteps)
                {
                    new PromotePieceStep(pawnId, PieceType.Bishop)
                };
                moves.Add(new GameplayMove(fromPosition, targetPosition, bishopStep));

                var knightSteps = new List<IGameplayStep>(baseSteps)
                {
                    new PromotePieceStep(pawnId, PieceType.Knight)
                };
                moves.Add(new GameplayMove(fromPosition, targetPosition, knightSteps));

                var QueenSteps = new List<IGameplayStep>(baseSteps)
                {
                    new PromotePieceStep(pawnId, PieceType.Queen)
                };
                moves.Add(new GameplayMove(fromPosition, targetPosition, QueenSteps));
            }
            else
            {
                var steps = new List<IGameplayStep>(baseSteps)
                {
                    new PlayerPromotionStep(pawnId)
                };

                moves.Add(new GameplayMove(fromPosition,targetPosition, steps));
            }
        }
    }
    
    public struct PieceData
    {
        public int Id;
        public bool IsColor; // true = white, false = black
        public PieceType PieceType;
        public int2 Position; // Replace Vector2Int with int2
        public bool HasMoved;
    }
    
    public struct BoardData
    {
        public NativeArray<PieceData> Pieces; // Fixed-size array (e.g. 32 max)
        public int TurnCount;
        public int EnPassantTurn;
        public int EnPassantPieceId;
        public int2 EnPassantTrapPosition;
    }

    public struct MoveData
    {
        public int2 TargetPosition;
        public int2 FromPosition;
        public MoveStepData? MoveStepData;
        public CaptureStepData? CaptureStepData;
        public PromoteStepData? PromoteStepData;
        public EnPassantStepData? EnPassantStepData;
        public CastlingMoveData? CastlingMoveData;
    }

    public struct MoveStepData
    {
        public int PieceToMoveId;
        public int2 TargetPosition;
    }

    public struct CaptureStepData
    {
        public int PieceCapturingId;
        public int PieceToCaptureId;
    }

    public struct PromoteStepData
    {
        public int PieceToPromoteId;
        public PromotionTypes PromotionType;
    }

    public struct EnPassantStepData
    {
        public int2 TargetPosition;
        public int TargetPieceId;
    }

    public struct CastlingMoveData
    {
        public int RookToCastlingId;
        public int KingToCastlingId;
        public int2 RookPosition;
        public int2 KingPosition;
    }
}


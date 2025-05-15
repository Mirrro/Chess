using Gameplay.Execution.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class PawnMoveGenerator
    {
        public static void GenerateMoves(ref PieceData piece, in BoardData board, bool isAi, NativeList<MoveData>.ParallelWriter output)
        {
            if (piece.PieceType != PieceType.Pawn)
                return;

            var direction = piece.IsColor ? 1 : -1;
            var promotionRank = piece.IsColor ? 7 : 0;

            // Forward 1
            var forward = piece.Position + new int2(0, direction);
            if (MoveGenerationHelperBurst.IsInBounds(forward) && !MoveGenerationHelperBurst.IsOccupied(board, forward))
            {
                var moveData = new MoveData
                {
                    TargetPosition = forward,
                    FromPosition = piece.Position,
                    
                    MoveStepData = new MoveStepData()
                    {
                        PieceToMoveId = piece.Id,
                        TargetPosition = forward
                    }
                };

                if (forward.y == promotionRank)
                {
                    MoveGenerationHelperBurst.EmitPromotions(moveData, piece.Id, isAi, output);
                }
                else
                {
                    output.AddNoResize(moveData);   
                }

                // Forward 2
                if (!piece.HasMoved)
                {
                    var doubleForward = piece.Position + new int2(0, 2 * direction);
                    if (MoveGenerationHelperBurst.IsInBounds(doubleForward) && !MoveGenerationHelperBurst.IsOccupied(board, doubleForward))
                    {
                        var doubleMoveData = new MoveData
                        {
                            TargetPosition = doubleForward,
                            FromPosition = piece.Position,
                            MoveStepData = new MoveStepData()
                            {
                                PieceToMoveId = piece.Id,
                                TargetPosition = doubleForward
                            },
                            EnPassantStepData = new EnPassantStepData()
                            {
                                TargetPosition = forward,
                                TargetPieceId = piece.Id,
                            }
                        };

                        output.AddNoResize(doubleMoveData);
                    }
                }
            }

            // Captures (left/right)
            for (int dx = -1; dx <= 1; dx += 2)
            {
                var diag = piece.Position + new int2(dx, direction);
                if (!MoveGenerationHelperBurst.IsInBounds(diag)) continue;

                var capturedId = MoveGenerationHelperBurst.GetEnemyAt(board, diag, piece.IsColor);
                if (capturedId != -1)
                {
                    var captureMoveData = new MoveData
                    {
                        TargetPosition = diag,
                        FromPosition = piece.Position,
                        MoveStepData = new MoveStepData()
                        {
                            PieceToMoveId = piece.Id,
                            TargetPosition = diag,
                        },
                        CaptureStepData = new CaptureStepData()
                        {
                            PieceCapturingId = piece.Id,
                            PieceToCaptureId = capturedId
                        }
                    };

                    if (diag.y == promotionRank)
                    {
                        MoveGenerationHelperBurst.EmitPromotions(captureMoveData, piece.Id, isAi, output);
                    }
                    else
                    {
                        output.AddNoResize(captureMoveData);
                    }
                }
            }

            // En Passant Capture
            if (board.TurnCount == board.EnPassantTurn + 1 &&
                math.distance(piece.Position, board.EnPassantTrapPosition) <= 1.5f &&
                MoveGenerationHelperBurst.GetEnemyAt(board, board.EnPassantTrapPosition, piece.IsColor) != -1)
            {
                var enpassantMoveData = new MoveData
                {
                    TargetPosition = board.EnPassantTrapPosition,
                    FromPosition = piece.Position,
                    MoveStepData = new MoveStepData()
                    {
                        PieceToMoveId = piece.Id,
                        TargetPosition = board.EnPassantTrapPosition
                    },
                    CaptureStepData = new CaptureStepData()
                    {
                        PieceCapturingId = piece.Id,
                        PieceToCaptureId = board.EnPassantPieceId,
                    }
                };

                output.AddNoResize(enpassantMoveData);
            }
        }
    }
}
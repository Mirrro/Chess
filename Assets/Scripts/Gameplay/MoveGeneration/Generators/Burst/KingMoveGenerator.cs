using Gameplay.Execution.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class KingMoveGenerator
    {
        private static readonly int2[] Offsets = new int2[]
        {
            new int2(-1,  1), // Up-Left
            new int2( 0,  1), // Up
            new int2( 1,  1), // Up-Right
            new int2( 1,  0), // Right
            new int2( 1, -1), // Down-Right
            new int2( 0, -1), // Down
            new int2(-1, -1), // Down-Left
            new int2(-1,  0)  // Left
        };

        public static void GenerateMoves(ref PieceData king, in BoardData board, NativeList<MoveData>.ParallelWriter output)
        {
            // Standard 1-square moves
            for (int i = 0; i < Offsets.Length; i++)
            {
                int2 target = king.Position + Offsets[i];

                if (!MoveGenerationHelperBurst.IsInBounds(target))
                    continue;

                if (!MoveGenerationHelperBurst.IsOccupied(board, target))
                {
                    output.AddNoResize(new MoveData
                    {
                        TargetPosition = target,
                        FromPosition = king.Position,
                        MoveStepData = new MoveStepData
                        {
                            PieceToMoveId = king.Id,
                            TargetPosition = target
                        }
                    });
                }
                else
                {
                    int capturedId = MoveGenerationHelperBurst.GetEnemyAt(board, target, king.IsColor);
                    if (capturedId != -1)
                    {
                        output.AddNoResize(new MoveData
                        {
                            TargetPosition = target,
                            FromPosition = king.Position,
                            MoveStepData = new MoveStepData
                            {
                                PieceToMoveId = king.Id,
                                TargetPosition = target
                            },
                            CaptureStepData = new CaptureStepData
                            {
                                PieceCapturingId = king.Id,
                                PieceToCaptureId = capturedId
                            }
                        });
                    }
                }
            }

            // Castling (no check/check-through-check validation here)
            if (!king.HasMoved)
            {
                for (int i = 0; i < board.Pieces.Length; i++)
                {
                    var rook = board.Pieces[i];
                    if (rook.PieceType != PieceType.Rook || rook.IsColor != king.IsColor || rook.HasMoved)
                        continue;

                    if (!MoveGenerationHelperBurst.IsClearLine(board, king.Position, rook.Position))
                        continue;

                    bool isRight = rook.Position.x > king.Position.x;

                    var newKingPos = isRight
                        ? new int2(6, king.Position.y)
                        : new int2(2, king.Position.y);

                    var newRookPos = isRight
                        ? new int2(5, rook.Position.y)
                        : new int2(3, rook.Position.y);

                    output.AddNoResize(new MoveData
                    {
                        TargetPosition = newKingPos,
                        FromPosition = king.Position,
                        CastlingMoveData = new CastlingMoveData()
                        {
                            RookToCastlingId = rook.Id,
                            KingToCastlingId = king.Id,
                            RookPosition = newRookPos,
                            KingPosition = newKingPos
                        }
                    });
                }
            }
        }
    }
}
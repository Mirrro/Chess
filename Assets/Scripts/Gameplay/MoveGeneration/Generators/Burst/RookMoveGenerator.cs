using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class RookMoveGenerator
    {
        private static readonly int2[] Directions = new int2[]
        {
            new int2(0, 1),   // Up
            new int2(0, -1),  // Down
            new int2(1, 0),   // Right
            new int2(-1, 0)   // Left
        };

        public static void GenerateMoves(ref PieceData rook, in BoardData board, NativeList<MoveData>.ParallelWriter output)
        {
            for (int d = 0; d < Directions.Length; d++)
            {
                var dir = Directions[d];
                var current = rook.Position + dir;

                while (MoveGenerationHelperBurst.IsInBounds(current))
                {
                    var occupant = MoveGenerationHelperBurst.GetPieceAt(current, board);

                    if (occupant == null)
                    {
                        // Empty tile — simple move
                        output.AddNoResize(new MoveData
                        {
                            TargetPosition = current,
                            FromPosition = rook.Position,
                            MoveStepData = new MoveStepData
                            {
                                PieceToMoveId = rook.Id,
                                TargetPosition = current
                            }
                        });

                        current += dir;
                    }
                    else if (occupant.Value.IsColor != rook.IsColor)
                    {
                        // Capture enemy and stop
                        output.AddNoResize(new MoveData
                        {
                            TargetPosition = current,
                            FromPosition = rook.Position,
                            MoveStepData = new MoveStepData
                            {
                                PieceToMoveId = rook.Id,
                                TargetPosition = current
                            },
                            CaptureStepData = new CaptureStepData
                            {
                                PieceCapturingId = rook.Id,
                                PieceToCaptureId = occupant.Value.Id
                            }
                        });
                        break;
                    }
                    else
                    {
                        // Friendly piece — stop
                        break;
                    }
                }
            }
        }
    }
}
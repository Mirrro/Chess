using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class QueenMoveGenerator
    {
        private static readonly int2[] Directions = new int2[]
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

        public static void GenerateMoves(ref PieceData queen, in BoardData board, NativeList<MoveData>.ParallelWriter output)
        {
            for (int d = 0; d < Directions.Length; d++)
            {
                var dir = Directions[d];
                var current = queen.Position + dir;

                while (MoveGenerationHelperBurst.IsInBounds(current))
                {
                    if (!MoveGenerationHelperBurst.IsOccupied(board, current))
                    {
                        output.AddNoResize(new MoveData
                        {
                            TargetPosition = current,
                            FromPosition = queen.Position,
                            MoveStepData = new MoveStepData
                            {
                                PieceToMoveId = queen.Id,
                                TargetPosition = current
                            }
                        });

                        current += dir;
                    }
                    else
                    {
                        int enemyId = MoveGenerationHelperBurst.GetEnemyAt(board, current, queen.IsColor);
                        if (enemyId != -1)
                        {
                            output.AddNoResize(new MoveData
                            {
                                TargetPosition = current,
                                FromPosition = queen.Position,
                                MoveStepData = new MoveStepData
                                {
                                    PieceToMoveId = queen.Id,
                                    TargetPosition = current
                                },
                                CaptureStepData = new CaptureStepData
                                {
                                    PieceCapturingId = queen.Id,
                                    PieceToCaptureId = enemyId
                                }
                            });
                        }
                        break;
                    }
                }
            }
        }
    }
}
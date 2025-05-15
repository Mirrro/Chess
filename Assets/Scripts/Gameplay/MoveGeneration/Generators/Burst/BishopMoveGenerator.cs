using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class BishopMoveGenerator
    {
        private static readonly int2[] Directions = new int2[]
        {
            new int2(-1,  1), // Up-Left
            new int2( 1,  1), // Up-Right
            new int2( 1, -1), // Down-Right
            new int2(-1, -1)  // Down-Left
        };

        public static void GenerateMoves(ref PieceData bishop, in BoardData board, NativeList<MoveData>.ParallelWriter output)
        {
            for (int d = 0; d < Directions.Length; d++)
            {
                var dir = Directions[d];
                var current = bishop.Position + dir;

                while (MoveGenerationHelperBurst.IsInBounds(current))
                {
                    if (!MoveGenerationHelperBurst.IsOccupied(board, current))
                    {
                        output.AddNoResize(new MoveData
                        {
                            TargetPosition = current,
                            FromPosition = bishop.Position,
                            MoveStepData = new MoveStepData
                            {
                                PieceToMoveId = bishop.Id,
                                TargetPosition = current
                            }
                        });

                        current += dir;
                    }
                    else
                    {
                        int enemyId = MoveGenerationHelperBurst.GetEnemyAt(board, current, bishop.IsColor);
                        if (enemyId != -1)
                        {
                            output.AddNoResize(new MoveData
                            {
                                TargetPosition = current,
                                FromPosition = bishop.Position,
                                
                                MoveStepData = new MoveStepData
                                {
                                    PieceToMoveId = bishop.Id,
                                    TargetPosition = current
                                },
                                CaptureStepData = new CaptureStepData
                                {
                                    PieceCapturingId = bishop.Id,
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
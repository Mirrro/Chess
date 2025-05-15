using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class KnightMoveGenerator
    {
        private static readonly int2[] Offsets = new int2[]
        {
            new int2(-1,  2),
            new int2( 1,  2),
            new int2(-1, -2),
            new int2( 1, -2),
            new int2(-2,  1),
            new int2(-2, -1),
            new int2( 2,  1),
            new int2( 2, -1)
        };

        public static void GenerateMoves(ref PieceData knight, in BoardData board, NativeList<MoveData>.ParallelWriter output)
        {
            for (int i = 0; i < Offsets.Length; i++)
            {
                int2 target = knight.Position + Offsets[i];

                if (!MoveGenerationHelperBurst.IsInBounds(target))
                    continue;

                if (!MoveGenerationHelperBurst.IsOccupied(board, target))
                {
                    output.AddNoResize(new MoveData
                    {
                        TargetPosition = target,
                        FromPosition = knight.Position,
                        MoveStepData = new MoveStepData
                        {
                            PieceToMoveId = knight.Id,
                            TargetPosition = target
                        }
                    });
                }
                else
                {
                    int capturedId = MoveGenerationHelperBurst.GetEnemyAt(board, target, knight.IsColor);
                    if (capturedId != -1)
                    {
                        output.AddNoResize(new MoveData
                        {
                            TargetPosition = target,
                            FromPosition = knight.Position,
                            MoveStepData = new MoveStepData
                            {
                                PieceToMoveId = knight.Id,
                                TargetPosition = target
                            },
                            CaptureStepData = new CaptureStepData
                            {
                                PieceCapturingId = knight.Id,
                                PieceToCaptureId = capturedId
                            }
                        });
                    }
                }
            }
        }
    }
}
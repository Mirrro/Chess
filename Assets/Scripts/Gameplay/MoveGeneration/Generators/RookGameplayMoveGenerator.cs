using System.Collections.Generic;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.MoveGeneration.Utility;
using UnityEngine;

namespace Gameplay.MoveGeneration.Generators
{
    /// <summary>
    /// Generates legal gameplay moves for rooks using horizontal and vertical directions.
    /// </summary>
    public static class RookGameplayMoveGenerator
    {
        public static List<IGameplayMove> GenerateMoves(GameplayStateModel gameplayStateModel, int rookId)
        {
            var moves = new List<IGameplayMove>();

            if (gameplayStateModel.TryGetPieceModelById(rookId, out var rookModel))
            {
                var directions = new List<Vector2Int>
                {
                    Vector2Int.zero.Up(),
                    Vector2Int.zero.Down(),
                    Vector2Int.zero.Right(),
                    Vector2Int.zero.Left()
                };

                var targetPositions = MoveGenerationHelper.FindLegalPositions(gameplayStateModel, rookId, directions);

                foreach (var moveablePosition in targetPositions.moveables)
                {
                    moves.Add(new GameplayMove(moveablePosition, new List<IGameplayStep>
                    {
                        new MovePieceStep(rookId, moveablePosition)
                    }));
                }

                foreach (var captureable in targetPositions.captureables)
                {
                    moves.Add(new GameplayMove(captureable.position, new List<IGameplayStep>
                    {
                        new MovePieceStep(rookId, captureable.position),
                        new CapturePieceStep(rookId, captureable.targetId)
                    }));
                }
            }

            return moves;
        }
    }
}
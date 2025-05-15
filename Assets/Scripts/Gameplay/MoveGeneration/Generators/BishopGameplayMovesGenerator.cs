using System.Collections.Generic;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.MoveGeneration.Utility;
using UnityEngine;

namespace Gameplay.MoveGeneration.Generators
{
    /// <summary>
    /// Generates legal gameplay moves for bishops in all 4 diagonal directions.
    /// </summary>
    public static class BishopGameplayMovesGenerator
    {
        public static List<IGameplayMove> GenerateMoves(GameplayStateModel gameplayStateModel, int bishopId)
        {
            var moves = new List<IGameplayMove>();

            if (!gameplayStateModel.TryGetPieceModelById(bishopId, out var bishopModel))
            {
                return moves;
            }

            var directions = new List<Vector2Int>
            {
                Vector2Int.zero.Up().Left(),
                Vector2Int.zero.Up().Right(),
                Vector2Int.zero.Down().Right(),
                Vector2Int.zero.Down().Left()
            };

            var targetPositions = MoveGenerationHelper.FindLegalPositions(gameplayStateModel, bishopId, directions);

            foreach (var moveablePosition in targetPositions.moveables)
            {
                moves.Add(new GameplayMove(bishopModel.Position, moveablePosition, new List<IGameplayStep>
                {
                    new MovePieceStep(bishopId, moveablePosition)
                }));
            }

            foreach (var captureable in targetPositions.captureables)
            {
                moves.Add(new GameplayMove(bishopModel.Position, captureable.position, new List<IGameplayStep>
                {
                    new MovePieceStep(bishopId, captureable.position),
                    new CapturePieceStep(bishopId, captureable.targetId)
                }));
            }

            return moves;
        }
    }
}
using System.Collections.Generic;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.MoveGeneration.Utility;
using UnityEngine;

namespace Gameplay.MoveGeneration.Generators
{
    /// <summary>
    /// Generates legal gameplay moves for queens.
    /// </summary>
    public static class QueenGameplayMoveGenerator
    {
        public static List<IGameplayMove> GenerateMoves(GameplayStateModel gameplayStateModel, int queenId)
        {
            var moves = new List<IGameplayMove>();

            if (!gameplayStateModel.TryGetPieceModelById(queenId, out var queenModel))
            {
                return moves;
            }

            var directions = new List<Vector2Int>
            {
                Vector2Int.zero.Up().Left(),
                Vector2Int.zero.Up(),
                Vector2Int.zero.Up().Right(),
                Vector2Int.zero.Right(),
                Vector2Int.zero.Down().Right(),
                Vector2Int.zero.Down(),
                Vector2Int.zero.Down().Left(),
                Vector2Int.zero.Left()
            };

            var targetPositions = MoveGenerationHelper.FindLegalPositions(gameplayStateModel, queenId, directions);

            foreach (var moveablePosition in targetPositions.moveables)
            {
                moves.Add(new GameplayMove(moveablePosition, new List<IGameplayStep>
                {
                    new MovePieceStep(queenId, moveablePosition)
                }));
            }

            foreach (var captureable in targetPositions.captureables)
            {
                moves.Add(new GameplayMove(captureable.position, new List<IGameplayStep>
                {
                    new MovePieceStep(queenId, captureable.position),
                    new CapturePieceStep(queenId, captureable.targetId)
                }));
            }

            return moves;
        }
    }
}
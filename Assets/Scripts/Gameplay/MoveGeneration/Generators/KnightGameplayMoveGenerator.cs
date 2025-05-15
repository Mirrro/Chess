using System.Collections.Generic;
using System.Linq;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.MoveGeneration.Utility;
using UnityEngine;

namespace Gameplay.MoveGeneration.Generators
{
    /// <summary>
    /// Generates legal gameplay moves for knights, handling their unique L-shaped movement.
    /// </summary>
    public static class KnightGameplayMoveGenerator
    {
        public static List<IGameplayMove> GenerateMoves(GameplayStateModel gameplayStateModel, int knightId)
        {
            var moves = new List<IGameplayMove>();

            if (gameplayStateModel.TryGetPieceModelById(knightId, out var knightModel))
            {
                var possiblePositions = new List<Vector2Int>
                {
                    knightModel.Position.Up().Up().Left(),
                    knightModel.Position.Up().Up().Right(),
                    knightModel.Position.Down().Down().Left(),
                    knightModel.Position.Down().Down().Right(),
                    knightModel.Position.Right().Right().Down(),
                    knightModel.Position.Right().Right().Up(),
                    knightModel.Position.Left().Left().Down(),
                    knightModel.Position.Left().Left().Up()
                };

                foreach (var possiblePosition in possiblePositions)
                {
                    if (!MoveGenerationHelper.IsInBounds(possiblePosition))
                    {
                        continue;
                    }

                    if (!MoveGenerationHelper.IsTileOccupied(gameplayStateModel, possiblePosition))
                    {
                        moves.Add(new GameplayMove(knightModel.Position, possiblePosition, new List<IGameplayStep>
                        {
                            new MovePieceStep(knightId, possiblePosition)
                        }));
                    }
                    else
                    {
                        var targetPiece = gameplayStateModel.PieceMap.Values.FirstOrDefault(piece =>
                            piece.Position == possiblePosition && piece.IsColor != knightModel.IsColor);
                        if (targetPiece != null)
                        {
                            moves.Add(new GameplayMove(knightModel.Position, possiblePosition, new List<IGameplayStep>
                            {
                                new MovePieceStep(knightId, possiblePosition),
                                new CapturePieceStep(knightId, targetPiece.Id)
                            }));
                        }
                    }
                }
            }

            return moves;
        }
    }
}
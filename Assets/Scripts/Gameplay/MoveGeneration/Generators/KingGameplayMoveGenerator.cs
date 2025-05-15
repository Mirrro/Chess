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
    /// Generates legal gameplay moves for kings including castling.
    /// </summary>
    public static class KingGameplayMoveGenerator
    {
        public static List<IGameplayMove> GenerateMoves(GameplayStateModel gameplayStateModel, int kingId)
        {
            var moves = new List<IGameplayMove>();

            if (!gameplayStateModel.TryGetPieceModelById(kingId, out var kingModel))
            {
                return moves;
            }

            // Move and Capture
            var possiblePositions = new List<Vector2Int>
            {
                kingModel.Position.Up().Left(),
                kingModel.Position.Up(),
                kingModel.Position.Up().Right(),
                kingModel.Position.Right(),
                kingModel.Position.Down().Right(),
                kingModel.Position.Down(),
                kingModel.Position.Down().Left(),
                kingModel.Position.Left()
            };

            foreach (var possiblePosition in possiblePositions)
            {
                if (!MoveGenerationHelper.IsInBounds(possiblePosition))
                {
                    continue;
                }

                if (!MoveGenerationHelper.IsTileOccupied(gameplayStateModel, possiblePosition))
                {
                    moves.Add(new GameplayMove(kingModel.Position, possiblePosition, new List<IGameplayStep>
                    {
                        new MovePieceStep(kingId, possiblePosition)
                    }));
                }
                else
                {
                    var targetPiece = gameplayStateModel.PieceMap.Values.FirstOrDefault(piece =>
                        piece.Position == possiblePosition && piece.IsColor != kingModel.IsColor);
                    if (targetPiece != null)
                    {
                        moves.Add(new GameplayMove(kingModel.Position, possiblePosition, new List<IGameplayStep>
                        {
                            new MovePieceStep(kingId, possiblePosition),
                            new CapturePieceStep(kingId, targetPiece.Id)
                        }));
                    }
                }
            }

            // Castling
            if (!kingModel.HasMoved)
            {
                var castlingRookModels =
                    gameplayStateModel.PieceMap.Values.Where(piece =>
                        piece.PieceType == PieceType.Rook && piece.IsColor == kingModel.IsColor && !piece.HasMoved);

                foreach (var castlingRookModel in castlingRookModels)
                {
                    if (!MoveGenerationHelper.IsLineOccupied(gameplayStateModel, kingModel.Position,
                            castlingRookModel.Position))
                    {
                        // Right Side Castle.
                        if (castlingRookModel.Position.x > kingModel.Position.x)
                        {
                            var newKingPosition = new Vector2Int(6, kingModel.Position.y);
                            var newRookPosition = new Vector2Int(5, castlingRookModel.Position.y);
                            moves.Add(new GameplayMove(kingModel.Position, newKingPosition, new List<IGameplayStep>
                            {
                                new MovePieceStep(kingId, newKingPosition),
                                new MovePieceStep(castlingRookModel.Id, newRookPosition)
                            }));
                        }
                        else
                        {
                            var newKingPosition = new Vector2Int(2, kingModel.Position.y);
                            var newRookPosition = new Vector2Int(3, castlingRookModel.Position.y);
                            moves.Add(new GameplayMove(kingModel.Position, newKingPosition, new List<IGameplayStep>
                            {
                                new MovePieceStep(kingId, newKingPosition),
                                new MovePieceStep(castlingRookModel.Id, newRookPosition)
                            }));
                        }
                    }
                }
            }

            return moves;
        }
    }
}
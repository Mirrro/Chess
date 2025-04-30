using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Execution.Models;
using UnityEngine;

namespace Gameplay.MoveGeneration.Utility
{
    /// <summary>
    /// Provides utilities for validating legal move positions, bounds, and piece collisions.
    /// </summary>
    public static class MoveGenerationHelper
    {
        public static (List<Vector2Int> moveables, List<(Vector2Int position, int targetId)> captureables)
            FindLegalPositions(
                GameplayStateModel gameplayStateModel, int pieceId, List<Vector2Int> directions)
        {
            var moveables = new List<Vector2Int>();
            var captureables = new List<(Vector2Int position, int targetId)>();

            if (gameplayStateModel.TryGetPieceModelById(pieceId, out var pieceModel))
            {
                foreach (var direction in directions)
                {
                    for (var i = 1; i < 8; i++)
                    {
                        var targetPosition = pieceModel.Position + direction * i;
                        if (!IsInBounds(targetPosition))
                        {
                            continue;
                        }

                        if (!IsTileOccupied(gameplayStateModel, targetPosition))
                        {
                            moveables.Add(targetPosition);
                        }
                        else
                        {
                            var targetPiece =
                                gameplayStateModel.PieceMap.Values.FirstOrDefault(piece =>
                                    piece.Position == targetPosition && piece.IsColor != pieceModel.IsColor);
                            if (targetPiece != null)
                            {
                                captureables.Add((targetPosition, targetPiece.Id));
                            }

                            break;
                        }
                    }
                }
            }

            return (moveables, captureables);
        }

        public static bool IsInBounds(Vector2Int position)
        {
            return position is {x: >= 0, y: >= 0} and {x: < 8, y: < 8};
        }

        public static bool IsTileOccupied(GameplayStateModel state, Vector2Int position)
        {
            return state.PieceMap.Values.Any(p => p.Position == position);
        }

        public static bool IsLineOccupied(GameplayStateModel gameplayStateModel, Vector2Int from, Vector2Int to)
        {
            var dx = to.x - from.x;
            var dy = to.y - from.y;

            // Validate: must be horizontal, vertical, or diagonal
            if (dx != 0 && dy != 0 && Math.Abs(dx) != Math.Abs(dy))
            {
                throw new ArgumentException("IsLineOccupied supports only horizontal, vertical, or diagonal lines.");
            }

            var stepX = Math.Sign(dx);
            var stepY = Math.Sign(dy);
            var steps = Math.Max(Math.Abs(dx), Math.Abs(dy)) - 1;

            for (var i = 1; i <= steps; i++)
            {
                var current = new Vector2Int(from.x + stepX * i, from.y + stepY * i);
                if (IsTileOccupied(gameplayStateModel, current))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
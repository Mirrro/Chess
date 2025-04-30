using UnityEngine;

namespace Gameplay.MoveGeneration.Utility
{
    /// <summary>
    /// Provides direction-based extensions to define chessboard positions more fluently.
    /// </summary>
    public static class PositionExtensions
    {
        public static Vector2Int Left(this Vector2Int position)
        {
            return new Vector2Int(position.x - 1, position.y);
        }

        public static Vector2Int Right(this Vector2Int position)
        {
            return new Vector2Int(position.x + 1, position.y);
        }

        public static Vector2Int Up(this Vector2Int position)
        {
            return new Vector2Int(position.x, position.y + 1);
        }

        public static Vector2Int Down(this Vector2Int position)
        {
            return new Vector2Int(position.x, position.y - 1);
        }
    }
}
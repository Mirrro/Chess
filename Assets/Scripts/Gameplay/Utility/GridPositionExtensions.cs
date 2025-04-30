using UnityEngine;

namespace Gameplay.Utility
{
    /// <summary>
    /// Provides extension methods for converting between grid (Vector2Int) and world (Vector3) positions.
    /// </summary>
    public static class GridPositionExtensions
    {
        /// <summary>
        /// Converts a world position (Vector3) to a grid position (Vector2Int).
        /// Assumes X and Z map to grid X and Y.
        /// </summary>
        public static Vector2Int WorldToGrid(this Vector3 position)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        }

        /// <summary>
        /// Converts a grid position (Vector2Int) back to world position (Vector3).
        /// Puts Y at 0 (ground level), and maps X and Y to X and Z in world space.
        /// </summary>
        public static Vector3 GridToWorld(this Vector2Int gridPos)
        {
            return new Vector3(gridPos.x, 0f, gridPos.y);
        }
    }
}
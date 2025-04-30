using UnityEngine;

namespace Gameplay.Presentation.Pieces
{
    /// <summary>
    /// Implementation of a visual model for a piece.
    /// </summary>
    public class PieceVisualModel : IPieceVisualModel
    {
        public Color Color { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public bool IsCaptured { get; set; }
    }
}
using UnityEngine;

namespace Gameplay.Presentation.Pieces
{
    /// <summary>
    /// Interface for the visual state data of a piece.
    /// </summary>
    public interface IPieceVisualModel
    {
        Color Color { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        public bool IsCaptured { get; set; }
    }
}
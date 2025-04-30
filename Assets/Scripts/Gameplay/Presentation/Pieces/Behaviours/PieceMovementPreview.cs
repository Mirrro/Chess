using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Abstract base class for previewing a chess piece's potential move position.
    /// </summary>
    public abstract class PieceMovementPreview : MonoBehaviour
    {
        public abstract void PreviewMoveTo(Vector3 position);
        public abstract void ClearPreview();
    }
}
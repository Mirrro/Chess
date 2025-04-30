using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Abstract base class for defining how a chess piece is captured and restored.
    /// </summary>
    public abstract class PieceCapture : MonoBehaviour
    {
        public abstract void Capture();
        public abstract void UnCapture();
    }
}
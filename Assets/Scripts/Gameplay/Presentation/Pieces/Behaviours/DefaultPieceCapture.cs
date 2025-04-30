using UnityEngine;

namespace Gameplay.Presentation.Pieces.Behaviours
{
    /// <summary>
    /// Handles the default capture behavior of a chess piece by hiding its renderer.
    /// </summary>
    public class DefaultPieceCapture : PieceCapture
    {
        [SerializeField] private Renderer renderer;

        public override void Capture()
        {
            renderer.enabled = false;
        }

        public override void UnCapture()
        {
            renderer.enabled = true;
        }
    }
}
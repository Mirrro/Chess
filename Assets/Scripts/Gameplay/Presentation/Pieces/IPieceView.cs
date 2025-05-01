using System;
using UnityEngine;

namespace Gameplay.Presentation.Pieces
{
    /// <summary>
    /// Defines the interface for a piece's visual view, handling movement, color, capture, and previews.
    /// </summary>
    public interface IPieceView : IDisposable
    {
        public void Move(Vector3 position, Action onCompleted);
        public void SetColor(Color color);
        public void Capture();
        public void UnCapture();
        public void PreviewMove(Vector2Int targetPosition);
        public void ClearPreview();
        public void PreviewCapture();
        public void SetTransforms(Vector3 position, Quaternion rotation);
    }
}
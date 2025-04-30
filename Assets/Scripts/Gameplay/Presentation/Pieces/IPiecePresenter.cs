using System;
using UnityEngine;

namespace Gameplay.Presentation.Pieces
{
    /// <summary>
    /// Interface for controlling the visual presentation and behavior of a single chess piece.
    /// </summary>
    public interface IPiecePresenter
    {
        public void Move(Vector3 position, Action onCompleted);
        public void SetColor(Color color);
        public void Capture();
        public void UnCapture();
        public void PreviewMove(Vector2Int targetPosition);
        public void ClearPreview();
        public void PreviewCapture();
        public void Promote();
        public void UnPromote();
        public void PreviewPromote();
    }
}
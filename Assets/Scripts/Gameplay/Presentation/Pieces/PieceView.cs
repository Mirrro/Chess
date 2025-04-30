using System;
using Gameplay.Presentation.Pieces.Behaviours;
using Gameplay.Utility;
using UnityEngine;

namespace Gameplay.Presentation.Pieces
{
    /// <summary>
    /// Implementation of a piece view that delegates visual logic to behavior components.
    /// </summary>
    public class PieceView : MonoBehaviour, IPieceView
    {
        [SerializeField] private Renderer renderer;

        [Header("Live Behaviours")] [SerializeField]
        private PieceMovement pieceMovement;

        [SerializeField] private PieceCapture pieceCapture;

        [Header("Preview Behaviours")] [SerializeField]
        private PieceMovementPreview pieceMovementPreview;

        [SerializeField] private PieceCapturePreview pieceCapturePreview;

        public void Move(Vector3 position, Action onCompleted)
        {
            pieceMovement.Move(position, onCompleted);
        }

        public void SetColor(Color color)
        {
            renderer.material.color = color;
        }

        public void Capture()
        {
            pieceCapture.Capture();
        }

        public void UnCapture()
        {
            pieceCapture.UnCapture();
        }

        public void PreviewMove(Vector2Int targetPosition)
        {
            pieceMovementPreview.PreviewMoveTo(targetPosition.GridToWorld());
        }

        public void PreviewCapture()
        {
            pieceCapturePreview.PreviewCapture();
        }

        public void ClearPreview()
        {
            pieceMovementPreview.ClearPreview();
            pieceCapturePreview.ClearPreview();
        }

        public void SetTransforms(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
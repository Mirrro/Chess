using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Presentation.Pieces
{
    /// <summary>
    /// Connects the visual model with the view, handles game-driven updates to the piece's appearance and state.
    /// </summary>
    public class PiecePresenter : IPiecePresenter
    {
        private readonly IPieceView view;
        private readonly IPieceVisualModel visualModel;

        public PiecePresenter(IPieceView view, IPieceVisualModel visualModel)
        {
            this.view = view;
            this.visualModel = visualModel;
        }

        public void Initialize()
        {
            view.SetTransforms(visualModel.Position, visualModel.Rotation);
            view.SetColor(visualModel.Color);

            if (visualModel.IsCaptured)
            {
                view.Capture();
            }
            else
            {
                view.UnCapture();
            }
        }

        public void Move(Vector3 position, Action onCompleted)
        {
            visualModel.Position = position;
            view.Move(position, onCompleted);
        }

        public void SetColor(Color color)
        {
            visualModel.Color = color;
            view.SetColor(color);
        }

        public void Capture()
        {
            visualModel.IsCaptured = true;
            view.Capture();
        }

        public void UnCapture()
        {
            visualModel.IsCaptured = false;
            view.UnCapture();
        }

        public void PreviewMove(Vector2Int targetPosition)
        {
            view.PreviewMove(targetPosition);
        }

        public void ClearPreview()
        {
            view.ClearPreview();
        }

        public void PreviewCapture()
        {
            view.PreviewCapture();
        }

        public void Promote()
        {
            Debug.Log("Promoted");
        }

        public void UnPromote()
        {
            Debug.Log("UnPromoted");
        }

        public void PreviewPromote()
        {
            Debug.Log("PreviewPromoted");
        }

        public class Factory : PlaceholderFactory<IPieceView, IPieceVisualModel, PiecePresenter> { }

        public void Dispose()
        {
            view.Dispose();
        }
    }
}
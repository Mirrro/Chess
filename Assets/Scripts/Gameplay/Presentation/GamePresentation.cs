using System;
using System.Collections.Generic;
using Gameplay.Execution.Models;
using Gameplay.Presentation.Pieces;
using Gameplay.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Presentation
{
    /// <summary>
    /// High-level manager that connects gameplay models to visual elements such as tiles and piece views.
    /// </summary>
    public class GamePresentation
    {
        public event Action<Vector2Int> TileSelected;
        public event Action<Vector2Int> TileHovered;
        public event Action<Vector2Int> TileUnhovered;

        private readonly GameViewContainer gameViewContainer;
        private readonly BoardView boardView;
        private readonly PiecePresenter.Factory piecePresenterFactory;

        private readonly Dictionary<int, IPiecePresenter> piecesPresenterTable = new();

        public GamePresentation(BoardView boardView, GameViewContainer gameViewContainer,
            PiecePresenter.Factory piecePresenterFactory)
        {
            this.boardView = boardView;
            this.gameViewContainer = gameViewContainer;
            this.piecePresenterFactory = piecePresenterFactory;
        }

        public void Initialize(GameplayStateModel gameplayStateModel)
        {
            boardView.InitializeBoard();
            boardView.TileSelected += OnTileSelected;
            boardView.TileHovered += OnTileHovered;
            boardView.TileUnhovered += OnTileUnhovered;

            foreach (var pieceModel in gameplayStateModel.PieceMap.Values)
            {
                var pieceView = CreatePieceView(pieceModel);
                pieceView.SetColor(pieceModel.IsColor ? Color.grey : Color.black);
                piecesPresenterTable.Add(pieceModel.Id, pieceView);
            }
        }

        public void HighlightTile(Vector2Int position, Color color)
        {
            boardView.HighlightTile(position, color);
        }

        public void UnhighlightTile(Vector2Int position)
        {
            boardView.UnhighlightTile(position);
        }

        public bool TryGetPiecePresenter(int pieceId, out IPiecePresenter piecePresenter)
        {
            return piecesPresenterTable.TryGetValue(pieceId, out piecePresenter);
        }

        public void UnhighlightAllTiles()
        {
            boardView.UnhighlightAll();
        }

        private void OnTileUnhovered(int arg1, int arg2)
        {
            TileUnhovered?.Invoke(new Vector2Int(arg1, arg2));
        }

        private void OnTileHovered(int arg1, int arg2)
        {
            TileHovered?.Invoke(new Vector2Int(arg1, arg2));
        }

        private void OnTileSelected(int arg1, int arg2)
        {
            TileSelected?.Invoke(new Vector2Int(arg1, arg2));
        }

        private IPiecePresenter CreatePieceView(PieceGameplayModel pieceGameplayModel)
        {
            var rotation = pieceGameplayModel.IsColor
                ? Quaternion.LookRotation(Vector3.forward)
                : Quaternion.LookRotation(Vector3.back);

            var position = pieceGameplayModel.Position.GridToWorld();

            var visualModel = new PieceVisualModel();
            visualModel.Position = position;
            visualModel.Rotation = rotation;
            visualModel.Color = pieceGameplayModel.IsColor ? Color.grey : Color.black;

            var presenter = pieceGameplayModel.PieceType switch
            {
                PieceType.Pawn => piecePresenterFactory.Create(
                    Object.Instantiate(gameViewContainer.GetPawnViewPrefab()), visualModel),

                PieceType.Rook => piecePresenterFactory.Create(
                    Object.Instantiate(gameViewContainer.GetRookViewPrefab()), visualModel),

                PieceType.Bishop => piecePresenterFactory.Create(
                    Object.Instantiate(gameViewContainer.GetBishopViewPrefab()), visualModel),

                PieceType.Knight => piecePresenterFactory.Create(
                    Object.Instantiate(gameViewContainer.GetKnightViewPrefab()), visualModel),

                PieceType.Queen => piecePresenterFactory.Create(
                    Object.Instantiate(gameViewContainer.GetQueenViewPrefab()), visualModel),

                PieceType.King => piecePresenterFactory.Create(
                    Object.Instantiate(gameViewContainer.GetKingViewPrefab()), visualModel),

                _ => throw new ArgumentOutOfRangeException(nameof(pieceGameplayModel.PieceType),
                    $"Unhandled pieceGameplay type: {pieceGameplayModel.PieceType}")
            };
            presenter.Initialize();
            return presenter;
        }
    }
}
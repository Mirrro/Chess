using System;
using UnityEngine;

namespace Gameplay.Presentation
{
    /// <summary>
    /// Manages the chessboard UI, tile initialization, and highlighting logic.
    /// ToDo: Refactor to plain class and implement SO for settings like color. Get tile prefab from
    /// <see cref="GameViewContainer" />>
    /// </summary>
    public class BoardView : MonoBehaviour
    {
        public event Action<int, int> TileSelected;
        public event Action<int, int> TileHovered;
        public event Action<int, int> TileUnhovered;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Color black;
        [SerializeField] private Color white;

        private BoardTileView[,] boardTileViews;

        public void HighlightTile(Vector2Int position, Color color)
        {
            boardTileViews[position.x, position.y].Highlight(color);
        }

        public void UnhighlightTile(Vector2Int position)
        {
            boardTileViews[position.x, position.y].Unhighlight();
        }

        public void UnhighlightAll()
        {
            foreach (var boardTileView in boardTileViews)
            {
                boardTileView.Unhighlight();
            }
        }

        public void InitializeBoard()
        {
            var boardTileViews = new BoardTileView[8, 8];
            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var view = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity)
                        .GetComponent<BoardTileView>();
                    view.transform.SetParent(transform);
                    view.SetColor((x + y) % 2 == 0 ? black : white);
                    var x1 = x;
                    var y1 = y;
                    view.Clicked += () => TileSelected?.Invoke(x1, y1);
                    view.Hovered += () => TileHovered?.Invoke(x1, y1);
                    view.Unhovered += () => TileUnhovered?.Invoke(x1, y1);
                    boardTileViews[x, y] = view;
                }
            }

            this.boardTileViews = boardTileViews;
        }
    }
}
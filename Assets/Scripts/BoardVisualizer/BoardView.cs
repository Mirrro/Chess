using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private GameObject boardFieldPrefab;
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private GameObject ruckPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject bishopPrefab;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject kingPrefab;
    [SerializeField] private Color black;
    [SerializeField] private Color white;

    public event Action<int, int> TileSelected;

    private bool hasDrawnBoard;

    private BoardTileView[,] boardTileViews;

    private Dictionary<IPieces, PieceView> pieceToViewMap = new Dictionary<IPieces, PieceView>();

    private BoardVisualization activeVisualization = new BoardVisualization(){PieceVisualizations = new List<PieceVisualization>()};

    public void HighlightField(int x, int y, Color color)
    {
        boardTileViews[x,y].Highlight(color).Forget();
    }
    
    public void UnhighlightField(int x, int y)
    {
        boardTileViews[x,y].Unhighlight().Forget();
    }

    public void UnhighlightAll()
    {
        foreach (var boardTileView in boardTileViews)
        {
            boardTileView.Unhighlight().Forget();
        }
    }
    
    private void DrawBoard(BoardState state)
    {
        BoardTileView[,] boardTileViews = new BoardTileView[8, 8];
        for (int x = 0; x < state.Fields.GetLength(0); x++)
        {
            for (int y = 0; y < state.Fields.GetLength(1); y++)
            {
                BoardTileView view = Instantiate(boardFieldPrefab, new Vector3(x, 0, y), Quaternion.identity)
                    .GetComponent<BoardTileView>();
                view.SetColor((x + y) % 2 == 0 ? black : white);
                var x1 = x;
                var y1 = y;
                view.Clicked += () => TileSelected?.Invoke(x1, y1);
                boardTileViews[x, y] = view;
            }
        }

        this.boardTileViews = boardTileViews;
        hasDrawnBoard = true;
    }

    public async UniTask VisualizeBoardState(BoardState state)
    {
        if (!hasDrawnBoard)
        {
            DrawBoard(state);
        }
        
        var newVisualization = CreateBoardVisualization(state: state);
        var cachedVisualization = activeVisualization;
        activeVisualization = newVisualization;
        
        await TransitionBoardVisualization(cachedVisualization, newVisualization);
    }

    private async UniTask TransitionBoardVisualization(BoardVisualization previous, BoardVisualization next)
    {
        List<PieceVisualization> piecesToMove = new List<PieceVisualization>();
        List<PieceVisualization> piecesToRemove = new List<PieceVisualization>();
        List<PieceVisualization> piecesToAdd = new List<PieceVisualization>();
        
        foreach (var pieceVisualization in next.PieceVisualizations)
        {
            if (previous.PieceVisualizations.Select(x => x.Piece).Contains(pieceVisualization.Piece))
            {
                // Is existing piece.
                var previousPiece = previous.PieceVisualizations.Find(x => x.Piece == pieceVisualization.Piece);
                if (previousPiece.Position != pieceVisualization.Position)
                {
                    piecesToMove.Add(pieceVisualization);
                }
            }
            else
            {
                piecesToAdd.Add(pieceVisualization);
            }
        }
        
        foreach (var pieceVisualization in previous.PieceVisualizations)
        {
            if (!next.PieceVisualizations.Select(x => x.Piece).Contains(pieceVisualization.Piece))
            {
                piecesToRemove.Add(pieceVisualization);
            }
        }

        foreach (var pieceVisualization in piecesToMove)
        {
            if (pieceToViewMap.TryGetValue(pieceVisualization.Piece, out var view))
            {
                await view.Move(pieceVisualization.Position);
            }
        }

        foreach (var pieceVisualization in piecesToRemove)
        {
            if (pieceToViewMap.TryGetValue(pieceVisualization.Piece, out var view))
            {
                await view.Hide();
                pieceToViewMap.Remove(pieceVisualization.Piece);
            }
        }
        
        foreach (var pieceVisualization in piecesToAdd)
        {
            var view = CreatePieceView(pieceVisualization);
            
            pieceToViewMap.Add(pieceVisualization.Piece, view);

            await view.Show();
        }
    }

    private BoardVisualization CreateBoardVisualization(BoardState state)
    {
        List<PieceVisualization> pieceVisualizations = new List<PieceVisualization>();
        
        for (int x = 0; x < state.Fields.GetLength(0); x++)
        {
            for (int y = 0; y < state.Fields.GetLength(1); y++)
            {
                if (state.Fields[x, y].Piece != null)
                {
                    pieceVisualizations.Add(CreatePieceVisualization(state.Fields[x, y].Piece, new Vector2Int(x,y)));
                }
            }
        }

        return new BoardVisualization {PieceVisualizations = pieceVisualizations};
    }

    private PieceVisualization CreatePieceVisualization(IPieces piece, Vector2Int position)
    {
        PieceVisualization visualization;

        visualization.Piece = piece;
        visualization.Position = position;
        return visualization;
    }

    private PieceView CreatePieceView(PieceVisualization visualization)
    {
        PieceView view;
        
        switch (visualization.Piece.Type)
        {
            case PieceTypes.Pawn:
                view = Instantiate(pawnPrefab, new Vector3(visualization.Position.x, 0, visualization.Position.y), Quaternion.identity).GetComponent<PieceView>();
                break;
            case PieceTypes.Ruck:
                view = Instantiate(ruckPrefab, new Vector3(visualization.Position.x, 0, visualization.Position.y), Quaternion.identity).GetComponent<PieceView>();
                break;
            case PieceTypes.Knight:
                view = Instantiate(knightPrefab, new Vector3(visualization.Position.x, 0, visualization.Position.y), Quaternion.identity).GetComponent<PieceView>();
                break;
            case PieceTypes.Bishop:
                view = Instantiate(bishopPrefab, new Vector3(visualization.Position.x, 0, visualization.Position.y), Quaternion.identity).GetComponent<PieceView>();
                break;
            case PieceTypes.Queen:
                view = Instantiate(queenPrefab, new Vector3(visualization.Position.x, 0, visualization.Position.y), Quaternion.identity).GetComponent<PieceView>();
                break;
            case PieceTypes.King:
                view = Instantiate(kingPrefab, new Vector3(visualization.Position.x, 0, visualization.Position.y), Quaternion.identity).GetComponent<PieceView>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        view.SetColor(visualization.Piece.IsWhite ? white : black);
        return view;
    }

    internal struct BoardVisualization
    {
        public List<PieceVisualization> PieceVisualizations;
    }
    
    internal struct PieceVisualization
    {
        public IPieces Piece;
        public Vector2Int Position;
    }
}

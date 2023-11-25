using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Player
{
    public Player(BoardView view)
    {
        boardVisualization = view;
    }
    private BoardView boardVisualization;
    
    private BoardState activeState;

    private Dictionary<Vector2Int, BoardState> positionToStateMap = new Dictionary<Vector2Int, BoardState>();
    private BoardState move = null;

    public async UniTask<BoardState> WaitForMove(BoardState activeState)
    {
        this.activeState = activeState;
        boardVisualization.TileSelected += OnTileSelectFirst;

        await UniTask.WaitUntil(() => move != null);
        var cached = move;
        move = null;
        return cached;
    }

    private async void OnTileSelectFirst(int x, int y)
    {
        if(activeState.Fields[x, y].Piece is not {IsWhite: true})
        {
            return;
        }

        var possibleMoves = await activeState.Fields[x, y].Piece.GetValidMoves(activeState);
        boardVisualization.TileSelected -= OnTileSelectFirst;
        boardVisualization.TileSelected += OnTileSelectSecond;
        boardVisualization.HighlightField(x,y, Color.yellow);
        positionToStateMap.Clear();
        foreach (var move in possibleMoves)
        {
            var position = FindPosition(move, activeState.Fields[x, y].Piece);
            positionToStateMap.Add(position, move);
            boardVisualization.HighlightField(position.x, position.y, Color.cyan);
        }
    }

    private void OnTileSelectSecond(int x, int y)
    {
        if (positionToStateMap.TryGetValue(new Vector2Int(x,y), out var move))
        {
            this.move = move;
            boardVisualization.UnhighlightAll();
            boardVisualization.TileSelected -= OnTileSelectSecond;
            boardVisualization.TileSelected -= OnTileSelectFirst;
        }
        else
        {
            boardVisualization.UnhighlightAll();
            boardVisualization.TileSelected -= OnTileSelectSecond;
            boardVisualization.TileSelected += OnTileSelectFirst;
        }
    }

    private Vector2Int FindPosition(BoardState state, IPieces piece)
    {
        for (int x = 0; x < state.Fields.GetLength(0); x++)
        {
            for (int y = 0; y < state.Fields.GetLength(1); y++)
            {
                if (state.Fields[x, y].Piece == piece)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return Vector2Int.zero;
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class KnightPiece : IPieces
{
    public KnightPiece(bool isWhite)
    {
        IsWhite = isWhite;
    }
    public bool IsWhite { get; set; }

    public PieceTypes Type => PieceTypes.Knight;

    public async UniTask<BoardState[]> GetValidMoves(BoardState state)
    {
        Vector2Int position = FindPosition(state);

        List<UniTask<BoardState>> moves = new List<UniTask<BoardState>>()
        {
            KnightMove(state, position, 1, 2),
            KnightMove(state, position, -1, 2),
            KnightMove(state, position, 2, 1),
            KnightMove(state, position, 2, -1),
            KnightMove(state, position, -2, -1),
            KnightMove(state, position, -2, 1),
            KnightMove(state, position, -1, -2),
            KnightMove(state, position, 1, -2),
        };

        var rawAfterMoveStates = await UniTask.WhenAll(moves);

        List<BoardState> afterMoveStates = new List<BoardState>();
        foreach (var move in rawAfterMoveStates)
        {
            if (move != null)
            {
                afterMoveStates.Add(move);
            }
        }

        return afterMoveStates.ToArray();
    }
    
    private async UniTask<BoardState> KnightMove(BoardState state, Vector2Int position, int x, int y)
    {
        return await UniTask.RunOnThreadPool(() =>
        {
            if (IsInsideBoard(state,position.x + x, position.y + y))
            {
                if (state.Fields[position.x + x, position.y + y].Piece == null || 
                    state.Fields[position.x + x, position.y + y].Piece.IsWhite != IsWhite)
                {
                    BoardState boardState = state.CopyBoard();
                    boardState.Fields[position.x + x, position.y + y].Piece = this;
                    boardState.Fields[position.x, position.y].Piece = null;
                 
                    return boardState;
                }
            }
            return null;
        });
    }
    
    private bool IsInsideBoard(BoardState state, int x, int y)
    {
        return x < state.Fields.GetLength(0) &&
               x >= 0 &&
               y < state.Fields.GetLength(1) &&
               y >= 0;
    }
    
    private Vector2Int FindPosition(BoardState state)
    {
        for (int x = 0; x < state.Fields.GetLength(0); x++)
        {
            for (int y = 0; y < state.Fields.GetLength(1); y++)
            {
                if (state.Fields[x, y].Piece == this)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return Vector2Int.zero;
    }
}

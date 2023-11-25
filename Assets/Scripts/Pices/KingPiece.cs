using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class KingPiece : IPieces
{
    public KingPiece(bool isWhite)
    {
        IsWhite = isWhite;
    }
    public bool IsWhite { get; set; }

    public PieceTypes Type => PieceTypes.King;

    public async UniTask<BoardState[]> GetValidMoves(BoardState state)
    {
        return await UniTask.RunOnThreadPool(() =>
        {
            Vector2Int position = FindPosition(state);

            List<BoardState> moves = new List<BoardState>()
            {
                KingMove(state, position, 1, 0),
                KingMove(state, position, -1, 0),
                KingMove(state, position, 0, 1),
                KingMove(state, position, 0, -1),
                KingMove(state, position, 1, 1),
                KingMove(state, position, -1, -1),
                KingMove(state, position, -1, 1),
                KingMove(state, position, 1, -1),
            };

            return moves.Where(move => move != null).ToArray();
        });
    }
    
    private BoardState KingMove(BoardState state, Vector2Int position, int x, int y)
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

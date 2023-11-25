using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class QueenPiece : IPieces
{
    public QueenPiece(bool isWhite)
    {
        IsWhite = isWhite;
    }
    public bool IsWhite { get; set; }
    public PieceTypes Type => PieceTypes.Queen;
     public async UniTask<BoardState[]> GetValidMoves(BoardState state)
    {
        Vector2Int position = FindPosition(state);

        List<UniTask<BoardState[]>> moves = new List<UniTask<BoardState[]>>()
        {
            QueenMove(state, position, 1, 1),
            QueenMove(state, position, 1, -1),
            QueenMove(state, position, -1, 1),
            QueenMove(state, position, -1, -1),
            QueenMove(state, position, 0, 1),
            QueenMove(state, position, 0, -1),
            QueenMove(state, position, -1, 0),
            QueenMove(state, position, 1, 0),
        };

        var rawAfterMoveStates = await UniTask.WhenAll(moves);

        List<BoardState> afterMoveStates = new List<BoardState>();
        foreach (var move in rawAfterMoveStates)
        {
            if (move != null)
            {
                afterMoveStates.AddRange(move);
            }
        }

        return afterMoveStates.ToArray();
    }
    
    private async UniTask<BoardState[]> QueenMove(BoardState state, Vector2Int position, int x, int y)
    {
        return await UniTask.RunOnThreadPool(() =>
        {
            List<BoardState> boardStates = new List<BoardState>();
            
            for (int i = 1; i < 8; i++)
            {
                if (IsInsideBoard(state,position.x + x * i, position.y + y * i))
                {
                    if (state.Fields[position.x + x * i, position.y + y * i].Piece == null ||
                        state.Fields[position.x + x * i, position.y + y * i].Piece.IsWhite != IsWhite)
                    {
                        BoardState boardState = state.CopyBoard();
                        boardState.Fields[position.x + x * i, position.y + y * i].Piece = this;
                        boardState.Fields[position.x, position.y].Piece = null;

                        boardStates.Add(boardState);

                        if (state.Fields[position.x + x * i, position.y + y * i].Piece != null && 
                            state.Fields[position.x + x * i, position.y + y * i].Piece?.IsWhite != IsWhite)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            
            return boardStates.ToArray();
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

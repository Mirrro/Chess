using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PawnPiece : IPieces
{
    public PawnPiece(bool isWhite)
    {
        IsWhite = isWhite;
    }
    public bool IsWhite { get; set; }

    public PieceTypes Type => PieceTypes.Pawn;

    public async UniTask<BoardState[]> GetValidMoves(BoardState state)
    {
        Vector2Int position = FindPosition(state);

        int forward = IsWhite ? 1 : -1;
        
        List<UniTask<BoardState>> moves = new List<UniTask<BoardState>>()
        {
            PawnMove(state, position, 0, forward),
            PawnAttack(state, position, 1,forward),
            PawnAttack(state, position, -1,forward),
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
    
    private async UniTask<BoardState> PawnMove(BoardState state, Vector2Int position, int x, int y)
    {
        return await UniTask.RunOnThreadPool(() =>
        {
            if (IsInsideBoard(state,position.x + x, position.y + y))
            {
                if (state.Fields[position.x + x, position.y + y].Piece == null)
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
    
    private async UniTask<BoardState> PawnAttack(BoardState state, Vector2Int position, int x, int y)
    {
        return await UniTask.RunOnThreadPool(() =>
        {
            if (IsInsideBoard(state,position.x + x, position.y + y))
            {
                if (state.Fields[position.x + x, position.y + y].Piece != null && 
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

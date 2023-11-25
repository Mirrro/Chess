
using System.Collections.Generic;
using UnityEngine;

public class TestPiece 
{
    // Can go up, right, down and left
    public bool IsWhite { get; set; }

    public TestPiece(bool isWhite)
    {
        IsWhite = isWhite;
    }

    // public BoardState[] GetValidMoves(BoardState state)
    // {
    //     Vector2Int position = FindPosition(state);
    //     List<BoardState> afterMoveStates = new List<BoardState>();
    //     
    //     if (position.x + 1 < state.Fields.GetLength(0))
    //     {
    //         if (state.Fields[position.x + 1, position.y].Piece?.IsWhite != IsWhite)
    //         {
    //             BoardState moveRightState = state.CopyBoard();
    //
    //             moveRightState.Fields[position.x + 1, position.y].Piece = this;
    //             moveRightState.Fields[position.x, position.y].Piece = null;
    //             afterMoveStates.Add(moveRightState);
    //         }
    //     }
    //     
    //     if (position.x - 1 >= 0)
    //     {
    //         if (state.Fields[position.x - 1, position.y].Piece?.IsWhite != IsWhite)
    //         {
    //             BoardState moveLeftState = state.CopyBoard();
    //
    //             moveLeftState.Fields[position.x - 1, position.y].Piece = this;
    //             moveLeftState.Fields[position.x, position.y].Piece = null;
    //             afterMoveStates.Add(moveLeftState);
    //         }
    //         
    //     }
    //     
    //     if (position.y + 1 < state.Fields.GetLength(1))
    //     {
    //         if (state.Fields[position.x, position.y + 1].Piece?.IsWhite != IsWhite)
    //         {
    //             BoardState moveUpState = state.CopyBoard();
    //
    //             moveUpState.Fields[position.x, position.y + 1].Piece = this;
    //             moveUpState.Fields[position.x, position.y].Piece = null;
    //             afterMoveStates.Add(moveUpState);
    //         }
    //         
    //     }
    //     
    //     if (position.y - 1 >= 0)
    //     {
    //         if (state.Fields[position.x , position.y - 1].Piece?.IsWhite != IsWhite)
    //         {
    //             BoardState moveDownState = state.CopyBoard();
    //
    //             moveDownState.Fields[position.x, position.y - 1].Piece = this;
    //             moveDownState.Fields[position.x, position.y].Piece = null;
    //             afterMoveStates.Add(moveDownState);
    //         }
    //         
    //     }
    //     
    //     return afterMoveStates.ToArray();
    // }

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
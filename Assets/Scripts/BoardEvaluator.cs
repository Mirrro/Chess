using System;

public class BoardEvaluator
{
    public Evaluation EvaluateState(BoardState state)
    {
        Evaluation evaluation = new Evaluation();
        
        foreach (var field in state.Fields)
        {
            if (field.Piece == null)
                continue;
            
            if (field.Piece.IsWhite)
            {
                evaluation.ScoreWhite += GetPieceValue(field.Piece);
            }
            else
            {
                evaluation.ScoreBlack += GetPieceValue(field.Piece);
            }
        }

        return evaluation;
    }
    
    public bool IsGameOver(BoardState state)
    {
        int kingsOnBoard = 0;
        foreach (var boardField in state.Fields)
        {
            if (boardField.Piece is {Type: PieceTypes.King})
            {
                kingsOnBoard++;
            }
        }

        return kingsOnBoard < 2;
    }

    private int GetPieceValue(IPieces piece)
    {
        return piece.Type switch
        {
            PieceTypes.Pawn => 1,
            PieceTypes.Knight => 3,
            PieceTypes.Bishop => 3,
            PieceTypes.Ruck => 5,
            PieceTypes.Queen => 9,
            PieceTypes.King => 100,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
public struct Evaluation
{
    public int ScoreBlack;
    public int ScoreWhite;
}


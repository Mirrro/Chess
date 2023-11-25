using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

public class BoardState
{
    public BoardField[,] Fields = new BoardField[,] { };
    
    public IPieces[] GetAllPieces
    {
        get
        {
            List<IPieces> pieces = new List<IPieces>();
            foreach (var boardField in Fields)
            {
                if (boardField.Piece != null)
                {
                    pieces.Add(boardField.Piece);
                }
            }

            return pieces.ToArray();
        }
    }

    public async UniTask<BoardState[]> GetAllValidMoves(bool color)
    {
        List<IPieces> validPices = new List<IPieces>();
        
        foreach (var boardField in Fields)
        {
            if (boardField.Piece != null && boardField.Piece.IsWhite == color)
            {
                validPices.Add(boardField.Piece);
            }
        }
        
        var moveTasks = validPices.Select(piece => piece.GetValidMoves(this)).ToArray();
        
        // Wait for all tasks to complete
        BoardState[][] resultsArray = await UniTask.WhenAll(moveTasks);

        // Flatten the array of arrays into a single array
        return resultsArray.SelectMany(result => result).ToArray();
    }
}
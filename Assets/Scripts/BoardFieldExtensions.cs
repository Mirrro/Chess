public static class BoardFieldExtensions
{
    public static BoardState CopyBoard(this BoardState sourceBoard)
    {
        BoardState newBoard = new BoardState();

        newBoard.Fields = new BoardField[sourceBoard.Fields.GetLength(0),sourceBoard.Fields.GetLength(1)];
        
        for (int x = 0; x < sourceBoard.Fields.GetLength(0); x++)
        {
            for (int y = 0; y < sourceBoard.Fields.GetLength(1); y++)
            {
                newBoard.Fields[x, y] = new BoardField();
                
                if (sourceBoard.Fields[x, y].Piece != null)
                {
                    newBoard.Fields[x, y].Piece = sourceBoard.Fields[x, y].Piece;
                }
            }
        }

        return newBoard;
    }
}
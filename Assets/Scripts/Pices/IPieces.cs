using Cysharp.Threading.Tasks;

public interface IPieces
{
    public bool IsWhite { get; set; }

    public PieceTypes Type { get; }
    public UniTask<BoardState[]> GetValidMoves(BoardState state);
}

public enum PieceTypes
{
    Pawn,
    Knight,
    Bishop,
    Ruck,
    Queen,
    King
}
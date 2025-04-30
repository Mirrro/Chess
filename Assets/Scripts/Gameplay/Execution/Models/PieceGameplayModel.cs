using UnityEngine;

namespace Gameplay.Execution.Models
{
    /// <summary>
    /// Contains the gameplay-related state of a single piece.
    /// </summary>
    public class PieceGameplayModel : ICloneableModel<PieceGameplayModel>
    {
        public PieceGameplayModel(int id, bool isColor, PieceType pieceType, Vector2Int position)
        {
            Id = id;
            IsColor = isColor;
            PieceType = pieceType;
            Position = position;
            HasMoved = false;
        }

        public PieceGameplayModel Clone()
        {
            return new PieceGameplayModel(Id, IsColor, PieceType, Position)
            {
                HasMoved = HasMoved
            };
        }

        public int Id;
        public bool IsColor;
        public Vector2Int Position;
        public bool HasMoved;
        public PieceType PieceType;
    }
}
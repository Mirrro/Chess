using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Execution.Models
{
    /// <summary>
    /// Represents the entire current state of the game, including piece positions and special rules like en passant.
    /// </summary>
    public class GameplayStateModel : ICloneableModel<GameplayStateModel>
    {
        public Dictionary<int, PieceGameplayModel> PieceMap = new();

        public Vector2Int EnPassantTrapPosition = new(-1, -1);
        public int EnPassantTurn = -1;
        public int EnPassantPieceId = -1;
        public int TurnCount;
        
        public bool TryGetPieceModelById(int id, out PieceGameplayModel gameplayModel)
        {
            return PieceMap.TryGetValue(id, out gameplayModel);
        }

        public void AddPiece(PieceGameplayModel pieceGameplay)
        {
            PieceMap.Add(pieceGameplay.Id, pieceGameplay);
        }

        public void RemovePiece(int id)
        {
            PieceMap.Remove(id);
        }

        public GameplayStateModel Clone()
        {
            var clone = new GameplayStateModel
            {
                EnPassantTurn = EnPassantTurn,
                EnPassantPieceId = EnPassantPieceId,
                EnPassantTrapPosition = EnPassantTrapPosition,
                TurnCount = TurnCount
            };
            foreach (var kvp in PieceMap)
            {
                clone.PieceMap.Add(kvp.Key, kvp.Value.Clone());
            }

            return clone;
        }
    }
}
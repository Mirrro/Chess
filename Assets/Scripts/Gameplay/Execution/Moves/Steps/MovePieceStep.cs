using Gameplay.Execution.Models;
using UnityEngine;

namespace Gameplay.Execution.Moves.Steps
{
    /// <summary>
    /// Moves a piece from its current position to a new position.
    /// </summary>
    public class MovePieceStep : IGameplayStep
    {
        public MovePieceStep(int pieceToMoveId, Vector2Int targetPosition)
        {
            PieceToMoveId = pieceToMoveId;
            TargetPosition = targetPosition;
        }

        public void ApplyTo(GameplayStateModel gameplayStateModel)
        {
            if (!gameplayStateModel.TryGetPieceModelById(PieceToMoveId, out var pieceModel))
            {
                return;
            }

            PreviousGameplayModel = pieceModel.Clone();
            pieceModel.Position = TargetPosition;
            pieceModel.HasMoved = true;
        }

        public void Undo(GameplayStateModel gameplayStateModel)
        {
            if (!gameplayStateModel.TryGetPieceModelById(PieceToMoveId, out var pieceModel))
            {
                return;
            }

            pieceModel.Position = PreviousGameplayModel.Position;
            pieceModel.HasMoved = PreviousGameplayModel.HasMoved;
        }

        public int PieceToMoveId;
        public Vector2Int TargetPosition;
        public PieceGameplayModel PreviousGameplayModel;
    }
}
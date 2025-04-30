using Gameplay.Execution.Models;
using UnityEngine;

namespace Gameplay.Execution.Moves.Steps
{
    /// <summary>
    /// Marks a position as a valid en passant capture square for the next turn.
    /// </summary>
    public class SetEnPassantTrapStep : IGameplayStep
    {
        public SetEnPassantTrapStep(Vector2Int targetPosition, int targetPieceId)
        {
            TargetPosition = targetPosition;
            TargetPieceId = targetPieceId;
        }

        public void ApplyTo(GameplayStateModel gameplayStateModel)
        {
            previousEnPassantTurn = gameplayStateModel.EnPassantTurn;
            previousPosition = gameplayStateModel.EnPassantTrapPosition;
            previousId = gameplayStateModel.EnPassantPieceId;

            gameplayStateModel.EnPassantTurn = gameplayStateModel.TurnCount;
            gameplayStateModel.EnPassantTrapPosition = TargetPosition;
            gameplayStateModel.EnPassantPieceId = TargetPieceId;
        }

        public void Undo(GameplayStateModel gameplayStateModel)
        {
            gameplayStateModel.EnPassantTurn = previousEnPassantTurn;
            gameplayStateModel.EnPassantTrapPosition = previousPosition;
            gameplayStateModel.EnPassantPieceId = previousId;
        }

        public Vector2Int TargetPosition;
        public int TargetPieceId;

        public int previousEnPassantTurn;
        public Vector2Int previousPosition;
        public int previousId;
    }
}
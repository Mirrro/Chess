using Gameplay.Execution.Models;

namespace Gameplay.Execution.Moves.Steps
{
    /// <summary>
    /// Removes a captured piece from the board and stores its state for undo.
    /// </summary>
    public class CapturePieceStep : IGameplayStep
    {
        private PieceGameplayModel pieceGameplayModel;

        public CapturePieceStep(int pieceCapturingId, int pieceToCaptureId)
        {
            PieceCapturingId = pieceCapturingId;
            PieceToCaptureId = pieceToCaptureId;
        }

        public void ApplyTo(GameplayStateModel gameplayStateModel)
        {
            if (!gameplayStateModel.TryGetPieceModelById(PieceToCaptureId, out var capturedModel))
            {
                return;
            }

            pieceGameplayModel = capturedModel;
            gameplayStateModel.RemovePiece(capturedModel.Id);
        }

        public void Undo(GameplayStateModel gameplayStateModel)
        {
            gameplayStateModel.AddPiece(pieceGameplayModel);
        }

        public int PieceCapturingId;
        public int PieceToCaptureId;
    }
}
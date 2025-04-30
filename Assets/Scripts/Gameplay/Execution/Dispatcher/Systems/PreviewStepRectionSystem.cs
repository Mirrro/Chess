using Gameplay.Execution.Moves.Steps;
using Gameplay.Presentation;

namespace Gameplay.Execution.Dispatcher.Systems
{
    /// <summary>
    /// Previews the effects of a move visually without changing gameplay state (used for move highlighting).
    /// </summary>
    public class PreviewStepRectionSystem : BaseGameplayStepReactionSystem
    {
        public PreviewStepRectionSystem(GamePresentation gamePresentation)
        {
            RegisterGameplayStep<MovePieceStep>(
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToMoveId, out var piecePresenter))
                    {
                        piecePresenter.PreviewMove(step.TargetPosition);
                    }

                    onComplete.Invoke();
                },
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToMoveId, out var piecePresenter))
                    {
                        piecePresenter.ClearPreview();
                    }

                    onComplete.Invoke();
                });

            RegisterGameplayStep<CapturePieceStep>(
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToCaptureId, out var piecePresenter))
                    {
                        piecePresenter.PreviewCapture();
                    }

                    onComplete.Invoke();
                },
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToCaptureId, out var piecePresenter))
                    {
                        piecePresenter.ClearPreview();
                    }

                    onComplete.Invoke();
                });

            RegisterGameplayStep<PromotePieceStep>(
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToPromoteId, out var piecePresenter))
                    {
                        piecePresenter.PreviewPromote();
                    }

                    onComplete.Invoke();
                },
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToPromoteId, out var piecePresenter))
                    {
                        piecePresenter.ClearPreview();
                    }

                    onComplete.Invoke();
                });
        }
    }
}
using Gameplay.Execution.Moves.Steps;
using Gameplay.Presentation;
using Gameplay.Utility;

namespace Gameplay.Execution.Dispatcher.Systems
{
    /// <summary>
    /// Updates the visual presentation of the game (e.g., piece movement, capture) in response to gameplay steps.
    /// </summary>
    public class GamePresentationStepReactionSystem : BaseGameplayStepReactionSystem
    {
        public GamePresentationStepReactionSystem(GamePresentation gamePresentation)
        {
            RegisterGameplayStep<MovePieceStep>(
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToMoveId, out var piecePresenter))
                    {
                        piecePresenter.Move(step.TargetPosition.GridToWorld(), onComplete);
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                },
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToMoveId, out var piecePresenter))
                    {
                        piecePresenter.Move(step.PreviousGameplayModel.Position.GridToWorld(), onComplete);
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                });

            RegisterGameplayStep<CapturePieceStep>(
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToCaptureId, out var piecePresenter))
                    {
                        piecePresenter.Capture();
                        onComplete.Invoke();
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                },
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToCaptureId, out var piecePresenter))
                    {
                        piecePresenter.UnCapture();
                        onComplete.Invoke();
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                });

            RegisterGameplayStep<PromotePieceStep>(
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToPromoteId, out var piecePresenter))
                    {
                        piecePresenter.Promote();
                        onComplete.Invoke();
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                },
                (step, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToPromoteId, out var piecePresenter))
                    {
                        piecePresenter.UnPromote();
                        onComplete.Invoke();
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                });
        }
    }
}
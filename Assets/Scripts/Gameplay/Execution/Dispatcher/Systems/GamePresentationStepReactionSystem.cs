using Gameplay.Execution.Models;
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
                (step, ctx, onComplete) =>
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
                (step, ctx, onComplete) =>
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
                (step, ctx, onComplete) =>
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
                (step, ctx, onComplete) =>
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
                (step, ctx, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToPromoteId, out var piecePresenter))
                    {
                        if (ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToPromoteId, out var pieceModel))
                        {
                            piecePresenter.Promote();
                            gamePresentation.UpdatePresentation(pieceModel);
                            onComplete.Invoke();
                        }
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                },
                (step, ctx, onComplete) =>
                {
                    if (gamePresentation.TryGetPiecePresenter(step.PieceToPromoteId, out var piecePresenter))
                    {
                        if (ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToPromoteId, out var pieceModel))
                        {
                            piecePresenter.UnPromote();
                            gamePresentation.UpdatePresentation(pieceModel);
                            onComplete.Invoke();
                        }
                        else
                        {
                            onComplete.Invoke();
                        }
                    }
                    else
                    {
                        onComplete.Invoke();
                    }
                });
            
            RegisterGameplayStep<PlayerPromotionStep>(
                (step, ctx, onComplete) =>
                {
                    // Player select promotion and inject promotion step.
                    void OnPromotionSelected(PieceType promotionType)
                    {
                        ctx.AddStep(new PromotePieceStep(step.PieceToPromoteId, promotionType));
                        onComplete.Invoke();
                    }
                    
                    gamePresentation.SelectPromotion(OnPromotionSelected);
                },
                (step, ctx, onComplete) =>
                {
                    onComplete.Invoke();
                });
        }
    }
}
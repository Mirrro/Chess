using Gameplay.Execution.Moves.Steps;
using UnityEngine;

namespace Gameplay.Execution.Dispatcher.Systems
{
    /// <summary>
    /// Logs step executions and undos for debugging purposes.
    /// </summary>
    public class DebugStepReactionSystem : BaseGameplayStepReactionSystem
    {
        public DebugStepReactionSystem()
        {
            RegisterGameplayStep<MovePieceStep>(
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.PieceToMoveId} moved to {step.TargetPosition}.");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    Debug.Log(
                        $"[Execution Debug] {step.PieceToMoveId} moved to {step.PreviousGameplayModel.Position}.");
                    onComplete.Invoke();
                });

            RegisterGameplayStep<CapturePieceStep>(
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.PieceCapturingId} captured {step.PieceToCaptureId}.");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.PieceCapturingId} un-captured {step.PieceToCaptureId}.");
                    onComplete.Invoke();
                });

            RegisterGameplayStep<PromotePieceStep>(
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.PieceToPromoteId} promoted to {step.PromotionType}.");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.PieceToPromoteId} un-promoted from {step.PromotionType}.");
                    onComplete.Invoke();
                });

            RegisterGameplayStep<SetEnPassantTrapStep>(
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.TargetPieceId} activates en passant on {step.TargetPosition}");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    Debug.Log($"[Execution Debug] {step.TargetPieceId} removes en passant on {step.TargetPosition}");
                    onComplete.Invoke();
                });
        }
    }
}
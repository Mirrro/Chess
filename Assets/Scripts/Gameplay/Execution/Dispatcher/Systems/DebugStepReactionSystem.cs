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
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToMoveId, out var pieceModel);
                    Debug.Log($"[Execution Debug] Apply: {step.PieceToMoveId} / {pieceModel.PieceType} moved to {step.TargetPosition} + {ctx.GameplayStateModel.TurnCount}.");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToMoveId, out var pieceModel);
                    Debug.Log(
                        $"[Execution Debug] Undo: {step.PieceToMoveId} / {pieceModel.PieceType} / {pieceModel.IsColor} moved to {step.PreviousGameplayModel.Position} + {ctx.GameplayStateModel.TurnCount}.");
                    onComplete.Invoke();
                });

            RegisterGameplayStep<CapturePieceStep>(
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceCapturingId, out var pieceModel);
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToCaptureId, out var captureModel);
                    Debug.Log($"[Execution Debug] Apply: {step.PieceCapturingId} / {pieceModel?.PieceType} / {pieceModel?.IsColor} captured {step.PieceToCaptureId} / {captureModel?.PieceType} / {captureModel?.IsColor} + {ctx.GameplayStateModel.TurnCount}.");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceCapturingId, out var pieceModel);
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToCaptureId, out var captureModel);
                    Debug.Log($"[Execution Debug] Undo: {step.PieceCapturingId} / {pieceModel?.PieceType} / {pieceModel?.IsColor} captured {step.PieceToCaptureId} / {captureModel?.PieceType} / {captureModel?.IsColor} + {ctx.GameplayStateModel.TurnCount}.");
                    onComplete.Invoke();
                });

            RegisterGameplayStep<PromotePieceStep>(
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToPromoteId, out var pieceModel);
                    Debug.Log($"[Execution Debug] Apply: {step.PieceToPromoteId} / {pieceModel.PieceType} promoted to {step.PromotionType} + {ctx.GameplayStateModel.TurnCount}.");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.PieceToPromoteId, out var pieceModel);
                    Debug.Log($"[Execution Debug] Undo: {step.PieceToPromoteId} / {pieceModel.PieceType} un-promoted from {step.PromotionType} + {ctx.GameplayStateModel.TurnCount}.");
                    onComplete.Invoke();
                });

            RegisterGameplayStep<SetEnPassantTrapStep>(
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.TargetPieceId, out var pieceModel);
                    Debug.Log($"[Execution Debug] Apply: {step.TargetPieceId} / {pieceModel.PieceType} activates en passant on {step.TargetPosition} + {ctx.GameplayStateModel.TurnCount}");
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    ctx.GameplayStateModel.TryGetPieceModelById(step.TargetPieceId, out var pieceModel);
                    Debug.Log($"[Execution Debug] Undo: {step.TargetPieceId} / {pieceModel.PieceType} removes en passant on {step.TargetPosition} + {ctx.GameplayStateModel.TurnCount}");
                    onComplete.Invoke();
                });
        }
    }
}
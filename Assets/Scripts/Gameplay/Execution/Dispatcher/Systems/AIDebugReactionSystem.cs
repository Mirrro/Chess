using System;
using System.Linq;
using System.Text;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves.Steps;
using UnityEngine;

namespace Gameplay.Execution.Dispatcher.Systems
{
    public class AIDebugReactionSystem : BaseGameplayStepReactionSystem
    {
        public AIDebugReactionSystem()
        {
            RegisterGameplayStep<MovePieceStep>(
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Apply: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Undo: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                });

            RegisterGameplayStep<CapturePieceStep>(
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Apply: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Undo: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                });

            RegisterGameplayStep<PromotePieceStep>(
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Apply: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Undo: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                });

            RegisterGameplayStep<SetEnPassantTrapStep>(
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Apply: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                },
                (step, ctx, onComplete) =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"Undo: {step.GetType()}");
                    builder.AppendLine();
                    builder.Append(BuildStateString(ctx.GameplayStateModel));
                    Debug.Log(builder.ToString());
                    onComplete.Invoke();
                });
        }

        private string BuildStateString(GameplayStateModel state)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                stringBuilder.AppendLine();
                for (int j = 0; j < 8; j++)
                {
                    var piece = state.PieceMap.FirstOrDefault(p => p.Value.Position == new Vector2Int(j, i)).Value;
                    char letter = 'o';
                    if (piece != null)
                    {
                        letter = GetPieceTypeString(piece.PieceType);
                        letter = piece.IsColor ? char.ToUpper(letter) : char.ToLower(letter);
                    }
                    
                    stringBuilder.Append(letter);
                }
            }
            return stringBuilder.ToString();
        }

        private char GetPieceTypeString(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Pawn => 'P',
                PieceType.Rook => 'R',
                PieceType.Knight => 'N',
                PieceType.Bishop => 'B',
                PieceType.Queen => 'Q',
                PieceType.King => 'K',
                _ => throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null)
            };
        }
    }
}
using System;
using System.Collections.Generic;
using Gameplay.Execution.Engine;
using Gameplay.Execution.Moves.Steps;

namespace Gameplay.Execution.Dispatcher.Systems
{
    /// <summary>
    /// Base implementation for gameplay step systems that react to step application and undo events.
    /// </summary>
    public abstract class BaseGameplayStepReactionSystem : IGameplayStepReactionSystem
    {
        private readonly Dictionary<Type, (Action<IGameplayStep, ExecutionEngineContext, Action> onApply, Action<IGameplayStep, ExecutionEngineContext, Action> onUndo)>
            handlers = new();

        public void OnGameplayStepApplied(IGameplayStep stepExecutionData, ExecutionEngineContext ctx, Action onComplete)
        {
            if (handlers.TryGetValue(stepExecutionData.GetType(), out var handler))
            {
                handler.onApply(stepExecutionData, ctx, onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        public void OnGameplayStepUndo(IGameplayStep stepExecutionData, ExecutionEngineContext ctx, Action onComplete)
        {
            if (handlers.TryGetValue(stepExecutionData.GetType(), out var handler))
            {
                handler.onUndo(stepExecutionData, ctx, onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        public IEnumerable<Type> GetObservedStepTypes()
        {
            return handlers.Keys;
        }

        protected void RegisterGameplayStep<T>(
            Action<T, ExecutionEngineContext, Action> onApply,
            Action<T, ExecutionEngineContext, Action> onUndo) where T : IGameplayStep
        {
            var stepType = typeof(T);

            handlers[stepType] = (
                (step, ctx, onComplete) => onApply((T) step, ctx, onComplete),
                (step, ctx, onComplete) => onUndo((T) step, ctx, onComplete)
            );
        }
    }
}
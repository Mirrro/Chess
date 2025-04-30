using System;
using System.Collections.Generic;
using Gameplay.Execution.Moves.Steps;

namespace Gameplay.Execution.Dispatcher.Systems
{
    /// <summary>
    /// Base implementation for gameplay step systems that react to step application and undo events.
    /// </summary>
    public abstract class BaseGameplayStepReactionSystem : IGameplayStepReactionSystem
    {
        private readonly Dictionary<Type, (Action<IGameplayStep, Action> onApply, Action<IGameplayStep, Action> onUndo)>
            handlers = new();

        public void OnGameplayStepApplied(IGameplayStep stepExecutionData, Action onComplete)
        {
            if (handlers.TryGetValue(stepExecutionData.GetType(), out var handler))
            {
                handler.onApply(stepExecutionData, onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        public void OnGameplayStepUndo(IGameplayStep stepExecutionData, Action onComplete)
        {
            if (handlers.TryGetValue(stepExecutionData.GetType(), out var handler))
            {
                handler.onUndo(stepExecutionData, onComplete);
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
            Action<T, Action> onApply,
            Action<T, Action> onUndo) where T : IGameplayStep
        {
            var stepType = typeof(T);

            handlers[stepType] = (
                (step, onComplete) => onApply((T) step, onComplete),
                (step, onComplete) => onUndo((T) step, onComplete)
            );
        }
    }
}
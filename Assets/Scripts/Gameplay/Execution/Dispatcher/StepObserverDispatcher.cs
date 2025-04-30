using System;
using System.Collections.Generic;
using Gameplay.Execution.Dispatcher.Systems;
using Gameplay.Execution.Moves.Steps;
using Zenject;

namespace Gameplay.Execution.Dispatcher
{
    /// <summary>
    /// Observes and notifies all relevant systems when gameplay steps are applied or undone.
    /// </summary>
    public class StepObserverDispatcher
    {
        public event Action Completed;
        private readonly Dictionary<Type, List<IGameplayStepReactionSystem>> stepTypeMap = new();

        public StepObserverDispatcher(IEnumerable<IGameplayStepReactionSystem> observers)
        {
            foreach (var observer in observers)
            {
                foreach (var stepType in observer.GetObservedStepTypes())
                {
                    if (!stepTypeMap.ContainsKey(stepType))
                    {
                        stepTypeMap[stepType] = new List<IGameplayStepReactionSystem>();
                    }

                    stepTypeMap[stepType].Add(observer);
                }
            }
        }

        public void OnStepAllied(IGameplayStep step, Action onComplete)
        {
            if (!stepTypeMap.TryGetValue(step.GetType(), out var observers) || observers.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var remaining = observers.Count;
            foreach (var observer in observers)
            {
                observer.OnGameplayStepApplied(step, () =>
                {
                    if (--remaining == 0)
                    {
                        onComplete?.Invoke();
                    }
                });
            }
        }

        public void OnStepUndo(IGameplayStep step, Action onComplete)
        {
            if (!stepTypeMap.TryGetValue(step.GetType(), out var observers) || observers.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var remaining = observers.Count;
            foreach (var observer in observers)
            {
                observer.OnGameplayStepUndo(step, () =>
                {
                    if (--remaining == 0)
                    {
                        onComplete?.Invoke();
                    }
                });
            }
        }

        public class Factory : PlaceholderFactory<IEnumerable<IGameplayStepReactionSystem>, StepObserverDispatcher> { }
    }
}
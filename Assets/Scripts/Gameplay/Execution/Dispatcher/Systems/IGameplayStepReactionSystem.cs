using System;
using System.Collections.Generic;
using Gameplay.Execution.Moves.Steps;

namespace Gameplay.Execution.Dispatcher.Systems
{
    /// <summary>
    /// Interface for systems that react to specific gameplay step types when applied or undone.
    /// </summary>
    public interface IGameplayStepReactionSystem
    {
        void OnGameplayStepApplied(IGameplayStep gameplayStep, Action onComplete);
        void OnGameplayStepUndo(IGameplayStep gameplayStep, Action onComplete);
        IEnumerable<Type> GetObservedStepTypes();
    }
}
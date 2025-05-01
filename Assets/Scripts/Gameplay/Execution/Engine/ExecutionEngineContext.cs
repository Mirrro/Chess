using System;
using System.Collections.Generic;
using Gameplay.Execution.Dispatcher;
using Gameplay.Execution.Dispatcher.Systems;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Zenject;

namespace Gameplay.Execution.Engine
{
    /// <summary>
    /// Executes all steps of a move sequentially and allows undoing them in reverse order.
    /// </summary>
    public class ExecutionEngineContext
    {
        public GameplayStateModel GameplayStateModel => gameplayStateModel;
        private readonly GameplayStateModel gameplayStateModel;
        private readonly StepObserverDispatcher dispatcher;
        private readonly Queue<IGameplayStep> stepQueue = new();
        private readonly Queue<IGameplayStep> executedSteps = new();

        public ExecutionEngineContext(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove,
            StepObserverDispatcher dispatcher)
        {
            this.gameplayStateModel = gameplayStateModel;
            this.dispatcher = dispatcher;

            foreach (var step in gameplayMove.GetSteps())
            {
                stepQueue.Enqueue(step);
            }
        }

        public void Execute(Action onComplete)
        {
            DispatchNextStepInternal(onComplete);
        }

        public void Undo(Action onComplete)
        {
            UndoNextStepInternal(onComplete);
        }

        public void AddStep(IGameplayStep step)
        {
            stepQueue.Enqueue(step);
        }

        private void DispatchNextStepInternal(Action onComplete)
        {
            if (stepQueue.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var step = stepQueue.Dequeue();
            step.ApplyTo(gameplayStateModel);

            dispatcher.OnStepAllied(step, this, () =>
            {
                executedSteps.Enqueue(step);
                DispatchNextStepInternal(onComplete);
            });
        }

        private void UndoNextStepInternal(Action onComplete)
        {
            if (executedSteps.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            var step = executedSteps.Dequeue();
            step.Undo(gameplayStateModel);

            dispatcher.OnStepUndo(step, this, () => { UndoNextStepInternal(onComplete); });
        }

        public class Factory : PlaceholderFactory<GameplayStateModel, IGameplayMove, StepObserverDispatcher,
            ExecutionEngineContext> { }
    }
}
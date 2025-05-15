using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Execution.Dispatcher;
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
        private GameplayStateModel gameplayStateModel;
        private StepObserverDispatcher dispatcher;
        
        private readonly Queue<IGameplayStep> stepQueue = new();
        private readonly Stack<IGameplayStep> executedSteps = new();

        public void Clear()
        {
            stepQueue.Clear();
            executedSteps.Clear();
        }

        public void Construct(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, StepObserverDispatcher stepObserverDispatcher)
        {
            dispatcher = stepObserverDispatcher;
            this.gameplayStateModel = gameplayStateModel;
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
                executedSteps.Push(step);
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

            var step = executedSteps.Pop();
            step.Undo(gameplayStateModel);

            dispatcher.OnStepUndo(step, this, () => { UndoNextStepInternal(onComplete); });
        }
    }

    public class ExecutionEngineContextPool
    {
        private List<ExecutionEngineContext> freeContexts = new();

        public ExecutionEngineContext Get(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, StepObserverDispatcher stepObserverDispatcher)
        {
            if (freeContexts.Count == 0)
            {
                freeContexts.Add(new ExecutionEngineContext());
            }

            var context = freeContexts.First();
            context.Construct(gameplayStateModel, gameplayMove, stepObserverDispatcher);
            freeContexts.Remove(context);
            return context;
        }

        public void Return(ExecutionEngineContext executionEngineContext)
        {
            executionEngineContext.Clear();
            freeContexts.Add(executionEngineContext);
        }
    }
}
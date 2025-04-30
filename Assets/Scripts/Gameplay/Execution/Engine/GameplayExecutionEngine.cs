using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Execution.Dispatcher;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Zenject;

namespace Gameplay.Execution.Engine
{
    /// <summary>
    /// Manages execution of gameplay moves and their undoing using an internal context stack.
    /// </summary>
    public class GameplayExecutionEngine
    {
        private readonly StepObserverDispatcher dispatcher;
        private readonly ExecutionEngineContext.Factory factory;

        private readonly List<ExecutionEngineContext> executedContext = new();

        public GameplayExecutionEngine(StepObserverDispatcher dispatcher, ExecutionEngineContext.Factory factory)
        {
            this.dispatcher = dispatcher;
            this.factory = factory;
        }

        public void Execute(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, Action onComplete)
        {
            var executionEngineContext = factory.Create(gameplayStateModel, gameplayMove, dispatcher);
            executionEngineContext.Execute(onComplete);
            executedContext.Add(executionEngineContext);
        }

        public void Undo(Action onComplete)
        {
            if (executedContext.Count <= 0)
            {
                return;
            }

            var lastExecution = executedContext.Last();
            executedContext.Remove(lastExecution);
            lastExecution.Undo(onComplete);
        }

        public class Factory : PlaceholderFactory<StepObserverDispatcher, GameplayExecutionEngine> { }
    }
}
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
        private readonly ExecutionEngineContextPool contextPool;

        private readonly List<ExecutionEngineContext> executedContext = new();

        public GameplayExecutionEngine(StepObserverDispatcher dispatcher, ExecutionEngineContextPool contextPool)
        {
            this.dispatcher = dispatcher;
            this.contextPool = contextPool;
        }

        public void Execute(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, Action onComplete)
        {
            var executionEngineContext = contextPool.Get(gameplayStateModel, gameplayMove, dispatcher);
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
            lastExecution.Undo(OnComplete);
            return;

            void OnComplete()
            {
                contextPool.Return(lastExecution);
                onComplete?.Invoke();
            }
        }

        public class Factory : PlaceholderFactory<StepObserverDispatcher, GameplayExecutionEngine> { }
    }

    // public class NewGameplayExecutionEngine
    // {
    //     private readonly DispatchHandler dispatcher;
    //     private readonly GameplayStateModel gameplayStateModel;
    //     
    //     private Stack<IGameplayMove> executionHistory = new();
    //     public NewGameplayExecutionEngine(DispatchHandler dispatcher, GameplayStateModel gameplayStateModel)
    //     {
    //         this.dispatcher = dispatcher;
    //         this.gameplayStateModel = gameplayStateModel;
    //     }
    //
    //     public void ApplyStep(IGameplayMove gameplayMove)
    //     {
    //         foreach (var gameplayStep in gameplayMove.GetSteps())
    //         {
    //             gameplayStep.ApplyTo(gameplayStateModel);
    //             dispatcher.OnStepApply(gameplayStep);
    //         }
    //         executionHistory.Push(gameplayMove);
    //     }
    //
    //     public void Undo()
    //     {
    //         var lastExecution = executionHistory.Pop();
    //         foreach (var gameplayStep in lastExecution.GetSteps().Reverse())
    //         {
    //             gameplayStep.Undo(gameplayStateModel);
    //             dispatcher.OnStepUndo(gameplayStep);
    //         }
    //     }
    // }
    //
    // public class DispatchHandler
    // {
    //     private Queue<DispatcherElement> dispatchQueue = new();
    //
    //     private bool isRunning;
    //     
    //     public void OnStepApply(IGameplayStep step)
    //     {
    //         dispatchQueue.Enqueue(new DispatcherElement()
    //         {
    //             Step = step,
    //             Type = DispatchType.Apply
    //         });
    //     }
    //
    //     public void OnStepUndo(IGameplayStep step)
    //     {
    //         dispatchQueue.Enqueue(new DispatcherElement()
    //         {
    //             Step = step,
    //             Type = DispatchType.Undo
    //         });
    //     }
    //
    //     private void ExecuteQueue()
    //     {
    //         if (isRunning)
    //         {
    //             return;
    //         }
    //
    //         while (dispatchQueue.Count > 0)
    //         {
    //             var nextDispatch = dispatchQueue.Dequeue();
    //             
    //         }
    //     }
    // }
    //
    // public class DispatcherElement
    // {
    //     public IGameplayStep Step;
    //     public DispatchType Type;
    // }
    //
    // public enum DispatchType
    // {
    //     Apply,
    //     Undo
    // }
}
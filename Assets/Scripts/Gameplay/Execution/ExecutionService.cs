using System;
using Gameplay.Execution.Builder;
using Gameplay.Execution.Engine;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using UnityEngine;
using Zenject;

namespace Gameplay.Execution
{
    /// <summary>
    /// Provides methods to execute and undo gameplay moves across different engines: Live, Preview, and AI.
    /// </summary>
    public class ExecutionService : IInitializable, IDisposable
    {
        private readonly GameplayExecutionEngineBuilder gameplayExecutionEngineBuilder;
        private GameplayExecutionEngine liveGameplayExecutionEngine;
        private GameplayExecutionEngine previewGameplayExecutionEngine;
        private GameplayExecutionEngine aiGameplayExecutionEngine;
        

        public ExecutionService(GameplayExecutionEngineBuilder gameplayExecutionEngineBuilder)
        {
            this.gameplayExecutionEngineBuilder = gameplayExecutionEngineBuilder;
        }

        public void Initialize()
        {
            liveGameplayExecutionEngine = gameplayExecutionEngineBuilder.BuildLive();
            previewGameplayExecutionEngine = gameplayExecutionEngineBuilder.BuildPreview();
            aiGameplayExecutionEngine = gameplayExecutionEngineBuilder.BuildAI();
        }

        public void ExecuteLive(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, Action onComplete)
        {
            liveGameplayExecutionEngine.Execute(gameplayStateModel, gameplayMove, onComplete);
        }

        public void ExecutePreview(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, Action onComplete)
        {
            previewGameplayExecutionEngine.Execute(gameplayStateModel, gameplayMove, onComplete);
        }

        public void UndoPreview(Action onComplete)
        {
            previewGameplayExecutionEngine.Undo(onComplete);
        }

        public void ExecuteAI(GameplayStateModel gameplayStateModel, IGameplayMove gameplayMove, Action onComplete)
        {
            aiGameplayExecutionEngine.Execute(gameplayStateModel, gameplayMove, onComplete);
        }

        public void UndoAI(Action onComplete)
        {
            aiGameplayExecutionEngine.Undo(onComplete);
        }

        public void Dispose()
        {
            liveGameplayExecutionEngine = null;
            previewGameplayExecutionEngine = null;
            aiGameplayExecutionEngine = null;
        }
    }
}
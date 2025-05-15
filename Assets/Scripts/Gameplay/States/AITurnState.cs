using System;
using Cysharp.Threading.Tasks;
using Gameplay.AI;
using Gameplay.Bootstrapping;
using Gameplay.Execution;

namespace Gameplay.States
{
    /// <summary>
    /// Represents the state where the AI selects and executes a move.
    /// </summary>
    public class AITurnState : IGameplayState
    {
        public event Action StateCompleted;

        private readonly ChessAi chessAi;
        private readonly GameplayContext gameplayContext;
        private readonly ExecutionService executionService;

        public AITurnState(ChessAi chessAi, GameplayContext gameplayContext, ExecutionService executionService)
        {
            this.chessAi = chessAi;
            this.gameplayContext = gameplayContext;
            this.executionService = executionService;
        }

        public void Activate()
        {
            ExecuteMove().Forget();
        }

        public void Deactivate() { }

        private async UniTask ExecuteMove()
        {
            var bestMove = await chessAi.FindBestMove(gameplayContext.GameplayStateModel.Clone(), false, 5);
            executionService.ExecuteLive(gameplayContext.GameplayStateModel, bestMove, () => StateCompleted?.Invoke());
        }
    }
}
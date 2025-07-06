using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.AI;
using Gameplay.Bootstrapping;
using Gameplay.Execution;
using Gameplay.Execution.Moves.Steps;
using Gameplay.Presentation.UI;

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
        private readonly OpponentUIPresenter opponentUIPresenter;

        public AITurnState(ChessAi chessAi, GameplayContext gameplayContext, ExecutionService executionService, OpponentUIPresenter opponentUIPresenter)
        {
            this.chessAi = chessAi;
            this.gameplayContext = gameplayContext;
            this.executionService = executionService;
            this.opponentUIPresenter = opponentUIPresenter;
        }

        public void Activate()
        {
            ExecuteMove().Forget();
        }

        public void Deactivate() { }

        private async UniTask ExecuteMove()
        {
            var bestMove = await chessAi.FindBestMove(gameplayContext.GameplayStateModel.Clone(), false, gameplayContext.OpponentConfig.SearchDepth);
            
            if (bestMove.GetSteps().Any(step => step is CapturePieceStep))
            {
                opponentUIPresenter.DisplayMessage(gameplayContext.OpponentConfig.GetCaptureInFavourQuote());
            }
            else if (bestMove.GetSteps().Any(step => step is PromotePieceStep))
            {
                opponentUIPresenter.DisplayMessage(gameplayContext.OpponentConfig.GetPromotionInFavourQuote());
            }
            else
            {
                opponentUIPresenter.DisplayMessage(gameplayContext.OpponentConfig.GetMoveQuote());
            }
            
            executionService.ExecuteLive(gameplayContext.GameplayStateModel, bestMove, () => StateCompleted?.Invoke());
        }
    }
}
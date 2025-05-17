using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.AI;
using Gameplay.Bootstrapping;
using Gameplay.Execution;
using Gameplay.Execution.Moves.Steps;
using Gameplay.Presentation;
using Gameplay.Presentation.UI;
using NUnit.Framework;

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
        private readonly OpponentConfig opponentConfig;

        public AITurnState(ChessAi chessAi, GameplayContext gameplayContext, ExecutionService executionService, OpponentUIPresenter opponentUIPresenter, OpponentConfig opponentConfig)
        {
            this.chessAi = chessAi;
            this.gameplayContext = gameplayContext;
            this.executionService = executionService;
            this.opponentUIPresenter = opponentUIPresenter;
            this.opponentConfig = opponentConfig;
        }

        public void Activate()
        {
            ExecuteMove().Forget();
        }

        public void Deactivate() { }

        private async UniTask ExecuteMove()
        {
            opponentUIPresenter.DisplayMessage(opponentConfig.GetThinkingQuote());
            var bestMove = await chessAi.FindBestMove(gameplayContext.GameplayStateModel.Clone(), false, opponentConfig.SearchDepth);
            
            if (bestMove.GetSteps().Any(step => step is CapturePieceStep))
            {
                opponentUIPresenter.DisplayMessage(opponentConfig.GetCaptureInFavourQuote());
            }
            else if (bestMove.GetSteps().Any(step => step is PromotePieceStep))
            {
                opponentUIPresenter.DisplayMessage(opponentConfig.GetPromotionInFavourQuote());
            }
            else
            {
                opponentUIPresenter.DisplayMessage(opponentConfig.GetMoveQuote());
            }
            
            executionService.ExecuteLive(gameplayContext.GameplayStateModel, bestMove, () => StateCompleted?.Invoke());
        }
    }
}
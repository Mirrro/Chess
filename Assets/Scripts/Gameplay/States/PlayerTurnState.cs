using System;
using System.Linq;
using Gameplay.Bootstrapping;
using Gameplay.Execution;
using Gameplay.Execution.Moves;
using Gameplay.Execution.Moves.Steps;
using Gameplay.Player;
using Gameplay.Presentation;
using Gameplay.Presentation.UI;
using UnityEngine;

namespace Gameplay.States
{
    /// <summary>
    /// Represents the state where the player selects and executes a move.
    /// </summary>
    public class PlayerTurnState : IGameplayState
    {
        public event Action StateCompleted;

        private readonly MoveSelectionService moveSelectionService;
        private readonly ExecutionService executionService;
        private readonly GameplayContext gameplayContext;
        private readonly OpponentUIPresenter opponentUIPresenter;
        private readonly OpponentConfig opponentConfig;

        public PlayerTurnState(MoveSelectionService moveSelectionService, ExecutionService executionService, GameplayContext gameplayContext, OpponentUIPresenter opponentUIPresenter, OpponentConfig opponentConfig)
        {
            this.moveSelectionService = moveSelectionService;
            this.executionService = executionService;
            this.gameplayContext = gameplayContext;
            this.opponentUIPresenter = opponentUIPresenter;
            this.opponentConfig = opponentConfig;
        }

        public void Activate()
        {
            Debug.Log("PlayerTurnState: Activate");
            moveSelectionService.PlayerSelectMove(OnMoveSelected);
        }

        private void OnMoveSelected(IGameplayMove selectedMove)
        {
            executionService.ExecuteLive(gameplayContext.GameplayStateModel, selectedMove, OnMoveComplete);
            if (selectedMove.GetSteps().Any(step => step is CapturePieceStep))
            {
                opponentUIPresenter.DisplayMessage(opponentConfig.GetCaptureAgainstQuote());
            }
            else if (selectedMove.GetSteps().Any(step => step is PlayerPromotionStep))
            {
                opponentUIPresenter.DisplayMessage(opponentConfig.GetPromotionAgainstQuote());
            }
        }

        public void Deactivate()
        {
            Debug.Log("PlayerTurnState: Deactivate");
        }

        private void OnMoveComplete()
        {
            StateCompleted?.Invoke();
        }
    }
}
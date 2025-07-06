using Gameplay.AI;
using Gameplay.Bootstrapping;
using Gameplay.MoveGeneration;
using Gameplay.MoveGeneration.Generators;
using UnityEngine;

namespace Gameplay.States
{
    /// <summary>
    /// Manages the turn-based state machine between the player and AI turns.
    /// </summary>
    public class GameplayStateMachine
    {
        private readonly PlayerTurnState playerTurnState;
        private readonly AITurnState aiTurnState;
        private readonly GameplayContext gameplayContext;
        private readonly BurstMoveFinder movesFinder;
        private readonly EndScreenPresenter endScreenPresenter;
        private readonly SavingService savingService;

        private IGameplayState activeGameplayState;

        public GameplayStateMachine(PlayerTurnState playerTurnState, AITurnState aiTurnState,
            GameplayContext gameplayContext, BurstMoveFinder movesFinder, EndScreenPresenter endScreenPresenter, SavingService savingService)
        {
            this.playerTurnState = playerTurnState;
            this.aiTurnState = aiTurnState;
            this.gameplayContext = gameplayContext;
            this.movesFinder = movesFinder;
            this.endScreenPresenter = endScreenPresenter;
            this.savingService = savingService;
        }

        public void StartGame()
        {
            playerTurnState.StateCompleted += OnPlayerStateCompleted;
            aiTurnState.StateCompleted += OnAiStateCompleted;
            SwitchTurns(gameplayContext.GameplayStateModel.TurnCount % 2 == 0 ? playerTurnState : aiTurnState);
        }

        private void OnPlayerStateCompleted()
        {
            if (IsGameOver(false))
            {
                Debug.Log("Game Over");
                endScreenPresenter.ShowWhiteWin();
                return;
            }
            SwitchTurns(aiTurnState);
        }

        private void OnAiStateCompleted()
        {
            if (IsGameOver(true))
            {
                Debug.Log("Game Over");
                endScreenPresenter.ShowBlackWin();
                return;
            }
            SwitchTurns(playerTurnState);
        }

        private void SwitchTurns(IGameplayState newGameplayState)
        {
            activeGameplayState?.Deactivate();
            activeGameplayState = newGameplayState;
            activeGameplayState.Activate();
            savingService.SaveGame(gameplayContext);
            gameplayContext.GameplayStateModel.TurnCount++;
        }

        private bool IsGameOver(bool isColor)
        {
            return HasNoLegalMoves(isColor) || BoardEvaluator.IsGameOver(gameplayContext.GameplayStateModel);
        }

        private bool HasNoLegalMoves(bool isColor)
        {
            var moves = movesFinder.RunJob(gameplayContext.GameplayStateModel, isColor, false);
            return moves.Count == 0;
        }
    }
}
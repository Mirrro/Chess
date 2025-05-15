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

        private IGameplayState activeGameplayState;

        public GameplayStateMachine(PlayerTurnState playerTurnState, AITurnState aiTurnState,
            GameplayContext gameplayContext, BurstMoveFinder movesFinder)
        {
            this.playerTurnState = playerTurnState;
            this.aiTurnState = aiTurnState;
            this.gameplayContext = gameplayContext;
            this.movesFinder = movesFinder;
        }

        public void StartGame()
        {
            playerTurnState.StateCompleted += OnPlayerStateCompleted;
            aiTurnState.StateCompleted += OnAiStateCompleted;
            SwitchTurns(playerTurnState);
        }

        private void OnPlayerStateCompleted()
        {
            if (IsGameOver(false))
            {
                Debug.Log("Game Over");
                return;
            }
            SwitchTurns(aiTurnState);
        }

        private void OnAiStateCompleted()
        {
            if (IsGameOver(true))
            {
                Debug.Log("Game Over");
                return;
            }
            
            SwitchTurns(playerTurnState);
        }

        private void SwitchTurns(IGameplayState newGameplayState)
        {
            var state = gameplayContext.GameplayStateModel;
            state.TurnCount++;
            gameplayContext.SetGameplayState(state);
            activeGameplayState?.Deactivate();
            activeGameplayState = newGameplayState;
            activeGameplayState.Activate();
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
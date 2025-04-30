using Gameplay.Bootstrapping;

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

        private IGameplayState activeGameplayState;

        public GameplayStateMachine(PlayerTurnState playerTurnState, AITurnState aiTurnState,
            GameplayContext gameplayContext)
        {
            this.playerTurnState = playerTurnState;
            this.aiTurnState = aiTurnState;
            this.gameplayContext = gameplayContext;
        }

        public void StartGame()
        {
            playerTurnState.StateCompleted += OnPlayerStateCompleted;
            aiTurnState.StateCompleted += OnAiStateCompleted;
            SwitchTurns(playerTurnState);
        }

        private void OnPlayerStateCompleted()
        {
            SwitchTurns(aiTurnState);
        }

        private void OnAiStateCompleted()
        {
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
    }
}
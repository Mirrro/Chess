using Gameplay.Execution.Models;
using Gameplay.Presentation;
using Gameplay.States;
using Zenject;

namespace Gameplay.Bootstrapping
{
    /// <summary>
    /// Initializes the game state, UI presentation, and starts the state machine when the game begins.
    /// </summary>
    public class Bootstrapper : IInitializable
    {
        private readonly GameplayContext gameplayContext;
        private readonly GamePresentation gamePresentation;
        private readonly GameplayStateMachine gameplayStateMachine;

        public Bootstrapper(GameplayContext gameplayContext, GamePresentation gamePresentation,
            GameplayStateMachine gameplayStateMachine)
        {
            this.gameplayContext = gameplayContext;
            this.gamePresentation = gamePresentation;
            this.gameplayStateMachine = gameplayStateMachine;
        }

        public void Initialize()
        {
            gameplayContext.SetGameplayState(GameplayStateModelCreationService.CreateNewGame());
            gamePresentation.Initialize(gameplayContext.GameplayStateModel);
            gameplayStateMachine.StartGame();
        }
    }
}
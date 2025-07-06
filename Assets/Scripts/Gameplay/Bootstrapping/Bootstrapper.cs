using Gameplay.Execution.Models;
using Gameplay.Presentation;
using Gameplay.Presentation.UI;
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
        private readonly OpponentUIPresenter opponentUIPresenter;

        public Bootstrapper(GameplayContext gameplayContext, GamePresentation gamePresentation,
            GameplayStateMachine gameplayStateMachine, OpponentUIPresenter opponentUIPresenter)
        {
            this.gameplayContext = gameplayContext;
            this.gamePresentation = gamePresentation;
            this.gameplayStateMachine = gameplayStateMachine;
            this.opponentUIPresenter = opponentUIPresenter;
        }

        public void Initialize()
        {
            opponentUIPresenter.SetOpponentPicture(gameplayContext.OpponentConfig.Sprite);
            gamePresentation.Initialize(gameplayContext.GameplayStateModel);
            gameplayStateMachine.StartGame();
        }
    }
}
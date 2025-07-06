using Gameplay.Execution.Models;
using Gameplay.Presentation;

namespace Gameplay.Bootstrapping
{
    /// <summary>
    /// Holds a reference to the current game state model for access across gameplay systems.
    /// </summary>
    public class GameplayContext
    {
        public OpponentConfig OpponentConfig { get; set; }
        public GameplayStateModel GameplayStateModel { get; private set; }

        public void SetGameplayState(GameplayStateModel newModel)
        {
            GameplayStateModel = newModel;
        }
    }
}
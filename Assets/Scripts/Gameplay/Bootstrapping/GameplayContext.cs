using Gameplay.Execution.Models;

namespace Gameplay.Bootstrapping
{
    /// <summary>
    /// Holds a reference to the current game state model for access across gameplay systems.
    /// </summary>
    public class GameplayContext
    {
        public GameplayStateModel GameplayStateModel { get; private set; }

        public void SetGameplayState(GameplayStateModel newModel)
        {
            GameplayStateModel = newModel;
        }
    }
}
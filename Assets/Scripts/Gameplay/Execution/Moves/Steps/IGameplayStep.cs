using Gameplay.Execution.Models;

namespace Gameplay.Execution.Moves.Steps
{
    /// <summary>
    /// Interface for a single atomic gameplay step that can be applied and undone.
    /// </summary>
    public interface IGameplayStep
    {
        public void ApplyTo(GameplayStateModel gameplayStateModel);
        public void Undo(GameplayStateModel gameplayStateModel);
    }
}
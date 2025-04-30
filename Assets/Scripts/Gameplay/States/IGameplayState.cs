using System;

namespace Gameplay.States
{
    /// <summary>
    /// Interface representing a single gameplay state with lifecycle methods and completion event.
    /// </summary>
    public interface IGameplayState
    {
        public event Action StateCompleted;
        public void Activate();
        public void Deactivate();
    }
}
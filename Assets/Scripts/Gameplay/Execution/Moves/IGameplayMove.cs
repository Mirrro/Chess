using System.Collections.Generic;
using Gameplay.Execution.Moves.Steps;
using UnityEngine;

namespace Gameplay.Execution.Moves
{
    /// <summary>
    /// Interface for any complete gameplay move composed of multiple steps and a target position.
    /// </summary>
    public interface IGameplayMove
    {
        public Vector2Int TargetPosition { get; }
        public IEnumerable<IGameplayStep> GetSteps();
    }
}
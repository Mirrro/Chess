using System.Collections.Generic;
using Gameplay.Execution.Moves.Steps;
using UnityEngine;

namespace Gameplay.Execution.Moves
{
    /// <summary>
    /// Represents a collection of gameplay steps that form a complete move to a target position.
    /// </summary>
    public class GameplayMove : IGameplayMove
    {
        public Vector2Int From { get; }
        public Vector2Int TargetPosition { get; }
        private readonly List<IGameplayStep> steps;

        public GameplayMove(Vector2Int from, Vector2Int targetPosition, List<IGameplayStep> steps)
        {
            From = from;
            TargetPosition = targetPosition;
            this.steps = steps;
        }

        public IEnumerable<IGameplayStep> GetSteps()
        {
            return steps;
        }
    }
}
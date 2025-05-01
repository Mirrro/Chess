using System;
using Gameplay.Bootstrapping;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.States
{
    /// <summary>
    /// Represents the state where the player selects and executes a move.
    /// </summary>
    public class PlayerTurnState : IGameplayState
    {
        public event Action StateCompleted;

        private readonly MoveSelectionService moveSelectionService;

        public PlayerTurnState(MoveSelectionService moveSelectionService, GameplayContext gameplayContext)
        {
            this.moveSelectionService = moveSelectionService;
        }

        public void Activate()
        {
            Debug.Log("PlayerTurnState: Activate");
            moveSelectionService.PlayerExecuteMove(OnMoveComplete);
        }

        public void Deactivate()
        {
            Debug.Log("PlayerTurnState: Deactivate");
        }

        private void OnMoveComplete()
        {
            StateCompleted?.Invoke();
        }
    }
}
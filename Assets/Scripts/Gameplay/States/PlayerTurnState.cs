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

        private readonly PlayerSelectionController playerSelectionController;

        public PlayerTurnState(PlayerSelectionController playerSelectionController, GameplayContext gameplayContext)
        {
            this.playerSelectionController = playerSelectionController;
        }

        public void Activate()
        {
            Debug.Log("PlayerTurnState: Activate");
            playerSelectionController.PlayerExecuteMove(OnMoveComplete);
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
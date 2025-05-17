using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Bootstrapping;
using Gameplay.Execution;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.MoveGeneration;
using Gameplay.MoveGeneration.Generators;
using Gameplay.Presentation;
using UnityEngine;

namespace Gameplay.Player
{
    /// <summary>
    /// Handles player interaction with the game board, including selecting pieces, previewing, and executing moves.
    /// </summary>
    public class MoveSelectionService
    {
        private readonly GamePresentation gamePresentation;
        private readonly GameplayContext gameplayContext;

        private readonly ExecutionService executionService;
        private readonly BurstMoveFinder movesFinder;

        private List<IGameplayMove> possibleGameplayMoves = new();
        private PieceGameplayModel selectedPieceGameplay;
        private Action<IGameplayMove> onMoveSelected;

        public MoveSelectionService(
            GamePresentation gamePresentation,
            GameplayContext gameplayContext,
            ExecutionService executionService)
        {
            this.gamePresentation = gamePresentation;
            this.gameplayContext = gameplayContext;
            this.executionService = executionService;
        }

        public void PlayerSelectMove(Action<IGameplayMove> onMoveSelected)
        {
            this.onMoveSelected = onMoveSelected;
            Deactivate();
            gamePresentation.TileSelected += OnTileSelected;
            gamePresentation.TileHovered += OnTileHovered;
            gamePresentation.TileUnhovered += OnTileUnhovered;
        }

        private void OnTileSelected(Vector2Int tilePosition)
        {
            if (selectedPieceGameplay == null)
            {
                selectedPieceGameplay = gameplayContext.GameplayStateModel.PieceMap.Values
                    .FirstOrDefault(p => p.Position == tilePosition);

                if (selectedPieceGameplay != null)
                {
                    possibleGameplayMoves = GameplayMovesGenerator.GetMovesForPiece(gameplayContext.GameplayStateModel,
                        selectedPieceGameplay.Id, false);

                    gamePresentation.UnhighlightAllTiles();
                    gamePresentation.HighlightTile(tilePosition, Color.yellow);
                    foreach (var move in possibleGameplayMoves)
                    {
                        gamePresentation.HighlightTile(move.TargetPosition, Color.cyan);
                    }
                }

                return;
            }

            var selectedMove = possibleGameplayMoves.FirstOrDefault(m => m.TargetPosition == tilePosition);
            if (selectedMove != null)
            {
                gamePresentation.UnhighlightAllTiles();
                Deactivate();
                onMoveSelected?.Invoke(selectedMove);
            }
            else
            {
                ClearSelection();
            }
        }

        private void OnTileHovered(Vector2Int tilePosition)
        {
            if (possibleGameplayMoves.Count == 0)
            {
                return;
            }

            var hoveredMove = possibleGameplayMoves.FirstOrDefault(m => m.TargetPosition == tilePosition);
            if (hoveredMove == null)
            {
                return;
            }

            executionService.ExecutePreview(gameplayContext.GameplayStateModel.Clone(), hoveredMove, null);
        }

        private void OnTileUnhovered(Vector2Int pos)
        {
            executionService.UndoPreview(null);
        }

        private void Deactivate()
        {
            gamePresentation.TileSelected -= OnTileSelected;
            gamePresentation.TileHovered -= OnTileHovered;
            gamePresentation.TileUnhovered -= OnTileUnhovered;
            ClearSelection();
        }

        private void ClearSelection()
        {
            selectedPieceGameplay = null;
            possibleGameplayMoves.Clear();
            gamePresentation.UnhighlightAllTiles();
            executionService.UndoPreview(null);
        }
    }
}
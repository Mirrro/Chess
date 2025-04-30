using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.MoveGeneration.Generators;

namespace Gameplay.MoveGeneration
{
    /// <summary>
    /// Provides methods to retrieve all legal moves for a given player based on board state.
    /// </summary>
    public static class GameplayMovesGenerator
    {
        public static List<IGameplayMove> GetMovesForPiece(GameplayStateModel gameplayStateModel, int pieceId)
        {
            var possibleGameplayMoves = new List<IGameplayMove>();

            if (!gameplayStateModel.TryGetPieceModelById(pieceId, out var pieceModel))
            {
                return possibleGameplayMoves;
            }

            possibleGameplayMoves = pieceModel.PieceType switch
            {
                PieceType.Pawn => PawnGameplayMoveGenerator.GeneratePawnMoves(gameplayStateModel, pieceModel.Id, false),
                PieceType.Rook => RookGameplayMoveGenerator.GenerateMoves(gameplayStateModel, pieceModel.Id),
                PieceType.Knight => KnightGameplayMoveGenerator.GenerateMoves(gameplayStateModel, pieceModel.Id),
                PieceType.Bishop => BishopGameplayMovesGenerator.GenerateMoves(gameplayStateModel, pieceModel.Id),
                PieceType.Queen => QueenGameplayMoveGenerator.GenerateMoves(gameplayStateModel, pieceModel.Id),
                PieceType.King => KingGameplayMoveGenerator.GenerateMoves(gameplayStateModel, pieceModel.Id),
                _ => throw new ArgumentOutOfRangeException()
            };
            return possibleGameplayMoves;
        }

        public static List<IGameplayMove> GetMoves(GameplayStateModel gameplayStateModel, bool isColor)
        {
            var moves = new List<IGameplayMove>();
            foreach (var pieceModel in gameplayStateModel.PieceMap.Values.Where(piece => piece.IsColor == isColor))
            {
                moves.AddRange(GetMovesForPiece(gameplayStateModel, pieceModel.Id));
            }

            return moves;
        }
    }
}
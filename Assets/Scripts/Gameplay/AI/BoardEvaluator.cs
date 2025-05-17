using System;
using System.Linq;
using Gameplay.Execution.Models;

namespace Gameplay.AI
{
    /// <summary>
    /// Evaluates a chess board state by assigning point values to pieces and checking for game-over conditions.
    /// </summary>
    public class BoardEvaluator
    {
        public static Evaluation EvaluateState(GameplayStateModel gameplayStateModel)
        {
            var evaluation = new Evaluation();

            foreach (var piece in gameplayStateModel.PieceMap.Values)
            {
                if (piece.IsColor)
                {
                    evaluation.ScoreWhite += GetPieceValue(piece.PieceType);
                }
                else
                {
                    evaluation.ScoreBlack += GetPieceValue(piece.PieceType);
                }
            }

            return evaluation;
        }

        public static bool IsGameOver(GameplayStateModel gameplayStateModel)
        {
            var kingsOnBoard = gameplayStateModel.PieceMap.Values.Count(piece => piece.PieceType == PieceType.King);
            return kingsOnBoard < 2;
        }

        private static int GetPieceValue(PieceType type)
        {
            return type switch
            {
                PieceType.Pawn => 10,
                PieceType.Rook => 50,
                PieceType.Knight => 30,
                PieceType.Bishop => 30,
                PieceType.Queen => 90,
                PieceType.King => 1000,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    /// <summary>
    /// Represents the evaluation result of a board state, storing scores for white and black sides.
    /// </summary>
    public struct Evaluation
    {
        public int ScoreBlack;
        public int ScoreWhite;
    }
}
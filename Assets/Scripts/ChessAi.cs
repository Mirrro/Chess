using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ChessAi
{
    private BoardEvaluator evaluator = new BoardEvaluator();

    public async UniTask<BoardState[]> FindBestMove(BoardState state, bool isColor, int depth)
    {
        int bestValue = int.MinValue;
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        List<BoardState> bestMoves = new List<BoardState>() {state};
        
        foreach (var possibleMove in await state.GetAllValidMoves(isColor))
        {
            int value = await Minimax(possibleMove, depth - 1, alpha, beta, !isColor, isColor);
            if (value >= bestValue)
            {
                if (value > bestValue)
                {
                    bestMoves.Clear();
                }
                
                bestValue = value;
                bestMoves.Add(possibleMove);
            }
        }
        return bestMoves.ToArray();
    }
    
    private async UniTask<int> Minimax(BoardState state, int depth, int alpha, int beta, bool isMaximizingPlayer, bool currentPlayer)
    {
        if (depth == 0 || evaluator.IsGameOver(state))
        {
            Evaluation evaluation = evaluator.EvaluateState(state);
            return currentPlayer ? evaluation.ScoreWhite - evaluation.ScoreBlack : evaluation.ScoreBlack - evaluation.ScoreWhite; 
        }

        int bestValue = isMaximizingPlayer == currentPlayer ? int.MinValue : int.MaxValue;
    
        foreach (var possibleMove in await state.GetAllValidMoves(isMaximizingPlayer))
        {
            int value = await Minimax(possibleMove, depth - 1, alpha, beta, !isMaximizingPlayer, currentPlayer);

            if (isMaximizingPlayer == currentPlayer)
            {
                bestValue = Math.Max(bestValue, value);
                beta = Math.Max(beta, value);
                if (beta <= alpha)
                {
                    break;
                }
            }
            else
            {
                bestValue = Math.Min(bestValue, value);
                alpha = Math.Min(alpha, value);
                if (beta <= alpha)
                {
                    break;
                }
            }
        }

        return bestValue;
    }
}

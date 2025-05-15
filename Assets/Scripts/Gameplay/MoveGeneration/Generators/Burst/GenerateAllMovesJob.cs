using System;
using Gameplay.Execution.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public struct GenerateAllMovesJob : IJobParallelFor
    {
        [ReadOnly] public BoardData Board;
        [ReadOnly] public bool IsAi;
        [ReadOnly] public bool IsColor;
        
        public NativeList<MoveData>.ParallelWriter OutputMoves;

        public void Execute(int index)
        {
            var piece = Board.Pieces[index];
            if (piece.IsColor != IsColor)
                return;

            switch (piece.PieceType)
            {
                case PieceType.Pawn:
                    PawnMoveGenerator.GenerateMoves(ref piece, Board, IsAi, OutputMoves);
                    break;
                case PieceType.Rook:
                    RookMoveGenerator.GenerateMoves(ref piece, Board, OutputMoves);
                    break;
                case PieceType.Knight:
                    KnightMoveGenerator.GenerateMoves(ref piece, Board, OutputMoves);
                    break;
                case PieceType.Bishop:
                    BishopMoveGenerator.GenerateMoves(ref piece, Board, OutputMoves);
                    break;
                case PieceType.Queen:
                    QueenMoveGenerator.GenerateMoves(ref piece, Board, OutputMoves);
                    break;
                case PieceType.King:
                    KingMoveGenerator.GenerateMoves(ref piece, Board, OutputMoves);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Gameplay.MoveGeneration.Generators
{
    [BurstCompile]
    public static class MoveGenerationHelperBurst
    {
        public static bool IsClearLine(in BoardData board, int2 a, int2 b)
        {
            int dx = math.sign(b.x - a.x);
            int dy = math.sign(b.y - a.y);

            int2 current = a + new int2(dx, dy);
            while (!current.Equals(b))
            {
                if (IsOccupied(board, current))
                    return false;
                current += new int2(dx, dy);
            }

            return true;
        }
        
        public static bool IsInBounds(int2 pos) => pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;

        public static bool IsOccupied(in BoardData board, int2 pos)
        {
            for (int i = 0; i < board.Pieces.Length; i++)
            {
                if (math.all(board.Pieces[i].Position == pos)) return true;
            }
            return false;
        }

        public static int GetEnemyAt(in BoardData board, int2 pos, bool isColor)
        {
            for (int i = 0; i < board.Pieces.Length; i++)
            {
                var p = board.Pieces[i];
                if (p.IsColor == isColor) continue;
                if (math.all(p.Position == pos)) return p.Id;
            }
            return -1;
        }

        public static bool IsEnemy(in PieceData piece, in PieceData potentialEnemy)
        {
            return piece.IsColor != potentialEnemy.IsColor;
        }
        
        public static PieceData? GetPieceAt(int2 pos, BoardData board)
        {
            for (int i = 0; i < board.Pieces.Length; i++)
            {
                var p = board.Pieces[i];
                if (p.Position.x == pos.x && p.Position.y == pos.y)
                    return p;
            }

            return null;
        }

        public static void EmitPromotions(MoveData moveData, int pieceId, bool isAi, NativeList<MoveData>.ParallelWriter output)
        {
            if (isAi)
            {
                moveData.PromoteStepData = new PromoteStepData
                {
                    PieceToPromoteId = pieceId, PromotionType = PromotionTypes.Queen
                };
                output.AddNoResize(moveData);

                moveData.PromoteStepData = new PromoteStepData
                {
                    PieceToPromoteId = pieceId, PromotionType = PromotionTypes.Rook
                };
                output.AddNoResize(moveData);

                moveData.PromoteStepData = new PromoteStepData
                {
                    PieceToPromoteId = pieceId, PromotionType = PromotionTypes.Bishop
                };
                output.AddNoResize(moveData);

                moveData.PromoteStepData = new PromoteStepData
                {
                    PieceToPromoteId = pieceId, PromotionType = PromotionTypes.Knight
                };
                output.AddNoResize(moveData);
            }
            else
            {
                moveData.PromoteStepData = new PromoteStepData
                {
                    PieceToPromoteId = pieceId,
                    PromotionType = PromotionTypes.Undefined
                };
                    
                output.AddNoResize(moveData);
            }
        }
    }
}
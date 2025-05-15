using Gameplay.Execution.Models;

namespace Gameplay.AI
{
    public static class ZobristHasher
    {
        public static ulong ComputeHashForState(GameplayStateModel state)
        {
            ulong hash = 0;

            foreach (var piece in state.PieceMap.Values)
            {
                int index = piece.Position.y * 8 + piece.Position.x;
                int color = piece.IsColor ? 1 : 0;

                hash ^= ZobristHashing.PieceHash[(int)piece.PieceType, color, index];

                if (piece.HasMoved)
                    hash ^= ZobristHashing.PieceHasMovedHash[piece.Id];
            }

            if (state.EnPassantTrapPosition.x >= 0 && state.EnPassantTrapPosition.y >= 0)
            {
                int epIndex = state.EnPassantTrapPosition.y * 8 + state.EnPassantTrapPosition.x;
                hash ^= ZobristHashing.EnPassantSquareHash[epIndex];
            }

            if (state.TurnCount % 2 == 1)
                hash ^= ZobristHashing.SideToMoveHash;

            return hash;
        }
    }
}
using System;
using Gameplay.Execution.Models;

namespace Gameplay.AI
{
    public static class ZobristHashing
    {
        public static readonly ulong[,,] PieceHash; // [pieceType][color][squareIndex]
        public static readonly ulong[] PieceHasMovedHash = new ulong[32];
        public static readonly ulong[] EnPassantSquareHash = new ulong[64];
        public static readonly ulong SideToMoveHash;

        static ZobristHashing()
        {
            System.Random rng = new System.Random(123456);

            int pieceTypes = Enum.GetValues(typeof(PieceType)).Length;
            PieceHash = new ulong[pieceTypes, 2, 64];

            for (int pt = 0; pt < pieceTypes; pt++)
            {
                for (int color = 0; color < 2; color++)
                {
                    for (int square = 0; square < 64; square++)
                    {
                        PieceHash[pt, color, square] = NextRandomUlong(rng);
                    }
                }
            }

            for (int i = 0; i < 32; i++)
                PieceHasMovedHash[i] = NextRandomUlong(rng);

            for (int i = 0; i < 64; i++)
                EnPassantSquareHash[i] = NextRandomUlong(rng);

            SideToMoveHash = NextRandomUlong(rng);
        }

        private static ulong NextRandomUlong(System.Random rng)
        {
            byte[] buffer = new byte[8];
            rng.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}
using UnityEngine;

namespace Gameplay.Execution.Models
{
    /// <summary>
    /// Creates an initial game board setup for starting a new chess match.
    /// </summary>
    public static class GameplayStateModelCreationService
    {
        public static GameplayStateModel CreateNewGame()
        {
            var gameplayState = new GameplayStateModel();
        
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(0,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(1,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(2,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(3,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(4,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(5,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(6,1)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Pawn, new Vector2Int(7,1)));
        
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Rook, new Vector2Int(0,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Knight, new Vector2Int(1,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Bishop, new Vector2Int(2,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Queen, new Vector2Int(3,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.King, new Vector2Int(4,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Bishop, new Vector2Int(5,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Knight, new Vector2Int(6,0)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), true, PieceType.Rook, new Vector2Int(7,0)));
        
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(0,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(1,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(2,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(3,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(4,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(5,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(6,6)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Pawn, new Vector2Int(7,6)));
        
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Rook, new Vector2Int(0,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Knight, new Vector2Int(1,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Bishop, new Vector2Int(2,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Queen, new Vector2Int(3,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.King, new Vector2Int(4,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Bishop, new Vector2Int(5,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Knight, new Vector2Int(6,7)));
            gameplayState.AddPiece(new PieceGameplayModel(PieceIdGenerator.Generate(), false, PieceType.Rook, new Vector2Int(7,7)));
            return gameplayState;
        }

        private static class PieceIdGenerator
        {
            private static int count;

            public static int Generate()
            {
                count++;
                return count;
            }
        }
    }
}
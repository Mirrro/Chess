using Gameplay.Execution.Models;

namespace Gameplay.Execution.Moves.Steps
{
    /// <summary>
    /// Promotes a piece to a different piece type and allows undoing the promotion.
    /// </summary>
    public class PromotePieceStep : IGameplayStep
    {
        public int PieceToPromoteId;
        public PieceType PromotionType;
        
        private PieceType previousType;

        public PromotePieceStep(int pieceToPromoteId, PieceType promotionType)
        {
            PieceToPromoteId = pieceToPromoteId;
            PromotionType = promotionType;
        }

        public void ApplyTo(GameplayStateModel gameplayStateModel)
        {
            if (!gameplayStateModel.TryGetPieceModelById(PieceToPromoteId, out var promotionModel))
            {
                return;
            }

            promotionModel.PieceType = PromotionType;
        }

        public void Undo(GameplayStateModel gameplayStateModel)
        {
            if (!gameplayStateModel.TryGetPieceModelById(PieceToPromoteId, out var promotionModel))
            {
                return;
            }

            promotionModel.PieceType = previousType;
        }
    }
    
    public class PlayerPromotionStep : IGameplayStep
    {
        public int PieceToPromoteId;
        public PlayerPromotionStep(int pieceToPromoteId)
        {
            PieceToPromoteId = pieceToPromoteId;
        }

        public void ApplyTo(GameplayStateModel gameplayStateModel)
        {
            
        }

        public void Undo(GameplayStateModel gameplayStateModel)
        {
            
        }
    }
}
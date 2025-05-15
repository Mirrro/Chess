using System;
using DG.Tweening;
using Gameplay.Execution.Models;
using UnityEngine;

public class PromotionMenu : MonoBehaviour
{
    public event Action<PieceType> PromotionClicked;
    
    [SerializeField] private PromotionButton knightButton;
    [SerializeField] private PromotionButton bishopButton;
    [SerializeField] private PromotionButton rookButton;
    [SerializeField] private PromotionButton queenButton;

    private Action knightHandler;
    private Action bishopHandler;
    private Action rookHandler;
    private Action queenHandler;
    
    private Sequence appearSequence;

    private void OnEnable()
    {
        knightHandler = () => OnPromotionClicked(PieceType.Knight);
        bishopHandler = () => OnPromotionClicked(PieceType.Bishop);
        rookHandler = () => OnPromotionClicked(PieceType.Rook);
        queenHandler = () => OnPromotionClicked(PieceType.Queen);

        knightButton.Clicked += knightHandler;
        bishopButton.Clicked += bishopHandler;
        rookButton.Clicked += rookHandler;
        queenButton.Clicked += queenHandler;
    }

    private void OnDisable()
    {
        knightButton.Clicked -= knightHandler;
        bishopButton.Clicked -= bishopHandler;
        rookButton.Clicked -= rookHandler;
        queenButton.Clicked -= queenHandler;
    }

    private void OnPromotionClicked(PieceType pieceType)
    {
        PromotionClicked?.Invoke(pieceType);
    }
}
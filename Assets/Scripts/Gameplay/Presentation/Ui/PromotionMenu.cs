using System;
using Gameplay.Execution.Models;
using UnityEngine;
using UnityEngine.UI;

public class PromotionMenu : MonoBehaviour
{
    public event Action<PieceType> PromotionClicked;

    [SerializeField] private Button knightButton;
    [SerializeField] private Button bishopButton;
    [SerializeField] private Button rookButton;
    [SerializeField] private Button queenButton;

    private void OnEnable()
    {
        knightButton.onClick.AddListener(() => PromotionClicked?.Invoke(PieceType.Knight));
        bishopButton.onClick.AddListener(() => PromotionClicked?.Invoke(PieceType.Bishop));
        rookButton.onClick.AddListener(() => PromotionClicked?.Invoke(PieceType.Rook));
        queenButton.onClick.AddListener(() => PromotionClicked?.Invoke(PieceType.Queen));
    }

    private void OnDisable()
    {
        knightButton.onClick.RemoveAllListeners();
        bishopButton.onClick.RemoveAllListeners();
        rookButton.onClick.RemoveAllListeners();
        queenButton.onClick.RemoveAllListeners();
    }
}

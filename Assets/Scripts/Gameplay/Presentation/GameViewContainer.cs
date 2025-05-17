using Gameplay.Presentation.Pieces;
using Gameplay.Presentation.UI;
using UnityEngine;

namespace Gameplay.Presentation
{
    /// <summary>
    /// Stores references to visual prefabs.
    /// </summary>
    [CreateAssetMenu(fileName = "GameViewContainer", menuName = "Game/GameViewContainer")]
    public class GameViewContainer : ScriptableObject
    {
        [SerializeField] private PieceView pawnViewPrefab;
        [SerializeField] private PieceView rookViewPrefab;
        [SerializeField] private PieceView knightViewPrefab;
        [SerializeField] private PieceView bishopViewPrefab;
        [SerializeField] private PieceView queenViewPrefab;
        [SerializeField] private PieceView kingViewPrefab;
        [SerializeField] private BoardTileView tileViewPrefab;
        [SerializeField] private MessageBoxView messageBoxPrefab;
        
        public MessageBoxView MessageBoxPrefab => messageBoxPrefab;

        public PieceView GetPawnViewPrefab()
        {
            return pawnViewPrefab;
        }

        public PieceView GetRookViewPrefab()
        {
            return rookViewPrefab;
        }

        public PieceView GetKnightViewPrefab()
        {
            return knightViewPrefab;
        }

        public PieceView GetBishopViewPrefab()
        {
            return bishopViewPrefab;
        }

        public PieceView GetQueenViewPrefab()
        {
            return queenViewPrefab;
        }

        public PieceView GetKingViewPrefab()
        {
            return kingViewPrefab;
        }

        public BoardTileView GetTileViewPrefab()
        {
            return tileViewPrefab;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Presentation
{
    [Serializable, CreateAssetMenu(fileName = "OpponentConfig", menuName = "Game/OpponentConfig")]
    public class OpponentConfig : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Color colorA = Color.gray;
        [SerializeField] private Color colorB = Color.white;
        
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private string title;
        
        [SerializeField] private List<string> moveQuotes = new List<string>();
        [SerializeField] private List<string> captureAgainstQuotes = new List<string>();
        [SerializeField] private List<string> thinkingQuote = new List<string>();
        [SerializeField] private List<string> captureInFavourQuote = new List<string>();
        [SerializeField] private List<string> promotionInFavourQuote = new List<string>();
        [SerializeField] private List<string> promotionAgainstQuote = new List<string>();
        
        [SerializeField] private int searchDepth;
        
        public Color ColorA => colorA;
        public Color ColorB => colorB;
        
        public Sprite Sprite => sprite;
        public string Name => name;
        public string Description => description;
        public string Title => title;
        public int SearchDepth => searchDepth;

        public string GetMoveQuote()
        {
            return GetRandomQuoteFrom(moveQuotes);
        }

        public string GetCaptureAgainstQuote()
        {
            return GetRandomQuoteFrom(captureAgainstQuotes);
        }

        public string GetThinkingQuote()
        {
            return GetRandomQuoteFrom(thinkingQuote);
        }

        public string GetCaptureInFavourQuote()
        {
            return GetRandomQuoteFrom(captureInFavourQuote);
        }

        public string GetPromotionInFavourQuote()
        {
            return GetRandomQuoteFrom(promotionInFavourQuote);
        }

        public string GetPromotionAgainstQuote()
        {
            return GetRandomQuoteFrom(promotionAgainstQuote);
        }

        private string GetRandomQuoteFrom(List<string> quotes)
        {
            return quotes.Count == 0 ? "" : quotes[UnityEngine.Random.Range(0, quotes.Count)];
        }
    }
}
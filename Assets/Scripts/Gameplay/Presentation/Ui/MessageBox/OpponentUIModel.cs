using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Presentation.UI
{
    public class OpponentUIModel
    {
        public Sprite Sprite { get; set; }
        public List<MessageData> Messages = new List<MessageData>(); 
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Presentation.UI
{
    public class OpponentUIView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Transform messageParent;
        
        public Transform MessageParent => messageParent;
        
        public void SetPicture(Sprite sprite)
        {
            image.sprite = sprite;    
        }
    }
}
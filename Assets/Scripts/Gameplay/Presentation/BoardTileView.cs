using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Presentation
{
    /// <summary>
    /// Handles interaction feedback on individual board tiles, including hover, click, and visual highlighting.
    /// </summary>
    public class BoardTileView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Clicked;
        public event Action Hovered;
        public event Action Unhovered;
        private static readonly int IsHighlight = Shader.PropertyToID("_IsHighlight");
        private static readonly int HighlightColor = Shader.PropertyToID("_HighlightColor");

        [SerializeField] private Renderer renderer;

        private Color color;

        private Vector3 initialPosition;

        public void Highlight(Color color)
        {
            renderer.material.SetColor(HighlightColor, color);
            renderer.material.SetInt(IsHighlight, 1);
            transform.position = initialPosition + new Vector3(0, 0.1f, 0);
        }

        public void Unhighlight()
        {
            renderer.material.SetInt(IsHighlight, 0);
            transform.position = initialPosition;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            renderer.material.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Clicked?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hovered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Unhovered?.Invoke();
        }

        private void Awake()
        {
            initialPosition = transform.position;
        }
    }
}
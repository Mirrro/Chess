using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PromotionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action Clicked;
    
    private Sequence hoverSequence;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverSequence?.Kill();
        hoverSequence = DOTween.Sequence();
        hoverSequence.Append(transform.DOScale(Vector3.one * 1.05f, 0.1f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverSequence?.Kill();
        hoverSequence = DOTween.Sequence();
        hoverSequence.Append(transform.DOScale(Vector3.one, 0.1f));
    }
}
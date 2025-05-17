using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gameplay.Presentation.UI
{
    public class MessageBoxView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageTextfield;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Sequence fadeTween;
        
        public void SetText(string text)
        {
            messageTextfield.text = text;
        }

        public void Show(Action callback)
        {
            canvasGroup.alpha = 0;
            fadeTween?.Kill();
            fadeTween = DOTween.Sequence();
            fadeTween.Append(canvasGroup.DOFade(1f, 1f));
            fadeTween.SetEase(Ease.Linear);
            fadeTween.OnComplete(() => callback?.Invoke());
        }

        public void Hide(Action callback)
        {
            fadeTween?.Kill();
            fadeTween = DOTween.Sequence();
            fadeTween.Append(canvasGroup.DOFade(0f, 1f));
            fadeTween.SetEase(Ease.Linear);
            fadeTween.OnComplete(() => callback?.Invoke());
        }
    }
}
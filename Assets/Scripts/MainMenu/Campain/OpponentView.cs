using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpponentView : MonoBehaviour
{
    [SerializeField] private RectTransform parentPanel;
    [SerializeField] private CanvasGroup NameCanvasGroup;
    [SerializeField] private CanvasGroup descriptionCanvasGroup;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    
    [SerializeField] private TMP_Text nameTextfield;
    [SerializeField] private TMP_Text descriptionTextfield;
    [SerializeField] private TMP_Text titleTextfield;
    [SerializeField] private Image profileImage;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button continueButton;

    private Sequence expansionSequence;
    public event Action Play;
    public event Action Continue;

    private void Start()
    {
        Collapse(null);
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlay);
        continueButton.onClick.AddListener(OnContinue);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
    }

    private void OnPlay()
    {
        Play?.Invoke();
    }

    private void OnContinue()
    {
        Continue?.Invoke();
    }

    public void SetName(string name)
    {
        nameTextfield.text = name;
    }

    public void SetDescription(string description)
    {
        descriptionTextfield.text = description;
    }

    public void SetProfile(Sprite profile)
    {
        profileImage.sprite = profile;
    }

    public void SetTitle(string title)
    {
        titleTextfield.text = title;
    }

    public void ActivateContinueButton(bool isActive)
    {
        continueButton.gameObject.SetActive(isActive);
    }

    public void Expand(Action onComplete)
    {
        expansionSequence?.Kill();
        expansionSequence = DOTween.Sequence();
        expansionSequence.Append(parentPanel.DOSizeDelta(new Vector2(1000, 500), .5f).SetEase(Ease.InCubic));
        expansionSequence.Append(NameCanvasGroup.DOFade(1, 0.1f));
        expansionSequence.Append(descriptionCanvasGroup.DOFade(1, 0.1f));
        expansionSequence.Append(buttonsCanvasGroup.DOFade(1, 0.1f));
        expansionSequence.OnComplete(() => onComplete?.Invoke());
    }

    public void Collapse(Action onComplete)
    {
        expansionSequence?.Kill();
        expansionSequence = DOTween.Sequence();
        expansionSequence.Append(descriptionCanvasGroup.DOFade(0, 0.1f));
        expansionSequence.Join(buttonsCanvasGroup.DOFade(0, 0.1f));
        expansionSequence.Join(NameCanvasGroup.DOFade(0, 0.1f));
        expansionSequence.Append(parentPanel.DOSizeDelta(new Vector2(350, 500), .5f).SetEase(Ease.OutCubic));
        expansionSequence.OnComplete(() => onComplete?.Invoke());
    }
}

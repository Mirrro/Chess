using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenView : MonoBehaviour
{
    [SerializeField] private TMP_Text textfield;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Button continueButton;
    
    public event Action OnContinue;

    private void OnEnable()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(OnContinueButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        OnContinue?.Invoke();
    }

    public void SetText(string text)
    {
        textfield.text = text;
    }

    public void SetVisible(bool visible)
    {
        endScreen.SetActive(visible);
    }
}
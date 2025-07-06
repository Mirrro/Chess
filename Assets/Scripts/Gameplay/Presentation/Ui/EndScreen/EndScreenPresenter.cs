using System;
using Zenject;

public class EndScreenPresenter : IInitializable, IDisposable
{
    private readonly EndScreenView view;
    private readonly SceneManagementService sceneManagementService;
    
    public EndScreenPresenter(EndScreenView view, SceneManagementService sceneManagementService)
    {
        this.view = view;
        this.sceneManagementService = sceneManagementService;
    }

    public void Initialize()
    {
        Hide();
        view.OnContinue += OnContinue;
    }

    public void Hide()
    {
        view.SetVisible(false);
    }

    public void ShowWhiteWin()
    {
        ShowPanel("White Win!");
    }

    public void ShowBlackWin()
    {
        ShowPanel("Black Win!");
    }

    private void ShowPanel(string text)
    {
        view.SetText(text);
        view.SetVisible(true);
    }

    private void OnContinue()
    {
        sceneManagementService.ToMainMenuScene();
    }

    public void Dispose()
    {
        view.OnContinue -= OnContinue;
    }
}
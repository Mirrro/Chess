using System.Collections.Generic;
using Gameplay.Bootstrapping;
using Gameplay.Presentation;
using UnityEngine.SceneManagement;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneManagementService>().AsSingle();
        Container.BindInterfacesAndSelfTo<SavingService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayContext>().AsSingle();
    }
}

public class MainMenu : IInitializable
{
    private readonly GameplayContext gameplayContext;
    private readonly CampaignMenuPresenter campaignMenuPresenter;
    
    [Inject(Id = "Opponent1")] private OpponentConfig opponentConfig1;
    [Inject(Id = "Opponent2")] private OpponentConfig opponentConfig2;
    [Inject(Id = "Opponent3")] private OpponentConfig opponentConfig3;

    public MainMenu(GameplayContext gameplayContext, CampaignMenuPresenter campaignMenuPresenter)
    {
        this.gameplayContext = gameplayContext;
        this.campaignMenuPresenter = campaignMenuPresenter;
    }
    
    public void Initialize()
    {
        gameplayContext.OpponentConfig = opponentConfig1;
        campaignMenuPresenter.SetOpponentData(new List<OpponentConfig>(){opponentConfig1, opponentConfig2, opponentConfig3});
    }
}

public class SceneManagementService
{
    public void ToGameplayScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

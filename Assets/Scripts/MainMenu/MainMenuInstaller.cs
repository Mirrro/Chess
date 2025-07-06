using Gameplay.Presentation;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private OpponentConfig opponentConfig1;
    [SerializeField] private OpponentConfig opponentConfig2;
    [SerializeField] private OpponentConfig opponentConfig3;
    [SerializeField] private CampaignMenuView campaignMenuView;
    [SerializeField] private OpponentView opponentView;
    
    public override void InstallBindings()
    {
        Container.BindInstance(opponentConfig1).WithId("Opponent1").AsCached();
        Container.BindInstance(opponentConfig2).WithId("Opponent2").AsCached();
        Container.BindInstance(opponentConfig3).WithId("Opponent3").AsCached();
        Container.BindInterfacesAndSelfTo<MainMenu>().AsSingle();
        Container.BindInterfacesAndSelfTo<CampaignMenuPresenter>().AsSingle().WithArguments(campaignMenuView, new CampaignMenuModel(), opponentView);
    }
}
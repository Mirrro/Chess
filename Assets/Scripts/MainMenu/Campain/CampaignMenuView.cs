using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Bootstrapping;
using Gameplay.Execution.Models;
using Gameplay.Presentation;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CampaignMenuView : MonoBehaviour
{
    private static readonly int ColorA = Shader.PropertyToID("_ColorA");
    private static readonly int ColorB = Shader.PropertyToID("_ColorB");
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Material backgroundMaterial;
    
    private Sequence colorSequence;

    public event Action LeftClicked;
    public event Action RightClicked;
    
    private void OnEnable()
    {
        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);
    }

    private void OnRightClick()
    {
        RightClicked?.Invoke();
    }

    private void OnLeftClick()
    {
        LeftClicked?.Invoke();
    }

    public void EnableLeft(bool enable)
    {
        leftButton.interactable = enable;
    }

    public void EnableRight(bool enable)
    {
        rightButton.interactable = enable;
    }

    public void SetColors(Color c1, Color c2)
    {
        colorSequence?.Kill();
        colorSequence = DOTween.Sequence();
        colorSequence.Join(backgroundMaterial.DOColor(c1, ColorA, 1));
        colorSequence.Join(backgroundMaterial.DOColor(c2, ColorB, 1));
    }
}

public class CampaignMenuPresenter : IInitializable, ITickable
{
    private readonly CampaignMenuView view;
    private readonly CampaignMenuModel model;
    
    private MoveState moveState;
    private ExpandState expandState;
    
    private ICampaignMenuState activeState;
    private OpponentView prefab;
    private SceneManagementService sceneManagementService;
    private GameplayContext gameplayContext;
    private SavingService savingService;

    public CampaignMenuPresenter(CampaignMenuView view, CampaignMenuModel model, OpponentView prefab)
    {
        this.view = view;
        this.model = model;
        this.prefab = prefab;
    }

    [Inject]
    public void Construct(SceneManagementService sceneManagementService, GameplayContext gameplayContext, SavingService savingService)
    {
        this.gameplayContext = gameplayContext;
        this.sceneManagementService = sceneManagementService;
        this.savingService = savingService;
    }
    
    public void Initialize()
    {
        expandState = new ExpandState(model);
        moveState = new MoveState(model);
        
        view.LeftClicked += OnLeftClicked;
        view.RightClicked += OnRightClicked;
    }

    public void SetOpponentData(List<OpponentConfig> opponentConfigs)
    {
        foreach (OpponentView opponentView in model.OpponentViews)
        {
            UnityEngine.Object.Destroy(opponentView.gameObject);
        }
        model.OpponentViews.Clear();
        model.OpponentConfigs.Clear();
        
        foreach (var opponentConfig in opponentConfigs)
        {
            model.OpponentConfigs.Add(opponentConfig);
            OpponentView opponentView = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, view.transform);
            opponentView.SetName(opponentConfig.Name);
            opponentView.SetDescription(opponentConfig.Description);
            opponentView.SetTitle(opponentConfig.Title);
            opponentView.SetProfile(opponentConfig.Sprite);
            opponentView.Play += () => StartGame(opponentConfig);
            opponentView.ActivateContinueButton(savingService.TryGetGameplayStateModel(out var gameplayStateModel, opponentConfig.Name));
            opponentView.Continue += () => ContinueGame(opponentConfig, gameplayStateModel);
            model.OpponentViews.Add(opponentView);
        }
    }

    private void StartGame(OpponentConfig config)
    {
        gameplayContext.SetGameplayState(GameplayStateModelCreationService.CreateNewGame());
        gameplayContext.OpponentConfig = config;
        sceneManagementService.ToGameplayScene();
    }

    private void ContinueGame(OpponentConfig opponentConfig, GameplayStateModel gameplayStateModel)
    {
        gameplayContext.SetGameplayState(gameplayStateModel);
        gameplayContext.OpponentConfig = opponentConfig;
        sceneManagementService.ToGameplayScene();
    }

    private void OnRightClicked()
    {
        if (model.TargetIndex >= model.OpponentViews.Count - 1)
        {
            return;
        }
        model.TargetIndex++;
    }

    private void OnLeftClicked()
    {
        if (model.TargetIndex <= 0)
        {
            return;
        }
        model.TargetIndex--;
    }

    public void Tick()
    {
        view.EnableRight(model.TargetIndex < model.OpponentViews.Count - 1);
        view.EnableLeft(model.TargetIndex > 0);
        if (model.TargetIndex == model.DisplayingIndex && activeState != expandState)
        {
            SwitchState(expandState);
        }
        
        view.SetColors(model.OpponentConfigs[model.DisplayingIndex].ColorA, model.OpponentConfigs[model.DisplayingIndex].ColorB);

        if (model.TargetIndex != model.DisplayingIndex && activeState != moveState)
        {
            SwitchState(moveState);
        }
    }

    private void SwitchState(ICampaignMenuState newState)
    {
        activeState?.Deactivate();
        activeState = newState;
        activeState?.Activate();
    }
}

public class CampaignMenuModel
{
    public List<OpponentConfig> OpponentConfigs = new List<OpponentConfig>();
    public List<OpponentView> OpponentViews = new List<OpponentView>();
    public int DisplayingIndex = 1;
    public int TargetIndex;
}

public class ExpandState : ICampaignMenuState
{
    private readonly CampaignMenuModel model;
    private OpponentView opponentView;
    
    public ExpandState(CampaignMenuModel model)
    {
        this.model = model;
    }

    public void Activate()
    {
        opponentView = model.OpponentViews[model.DisplayingIndex];
        opponentView.Expand(null);
    }

    public void Deactivate()
    {
        opponentView.Collapse(null);
    }
}

public class MoveState : ICampaignMenuState
{
    private readonly CampaignMenuModel model;
    private Sequence animationSequence;

    public MoveState(CampaignMenuModel model)
    {
        this.model = model;
    }

    public void Activate()
    {
        ToNext();
    }

    private void ToNext()
    {
        animationSequence?.Kill();
        animationSequence = DOTween.Sequence();
        int nextDisplayIndex = model.DisplayingIndex > model.TargetIndex ? model.DisplayingIndex-1 : model.DisplayingIndex+1;
        for (int i = 0; i < model.OpponentViews.Count; i++)
        {
            animationSequence.Join(model.OpponentViews[i].GetComponent<RectTransform>().DOLocalMove(new Vector3(GetPosition(i - nextDisplayIndex), 0, 0), .5f).SetEase(Ease.InOutCubic));
        }

        animationSequence.OnComplete(() =>
        {
            model.DisplayingIndex += model.DisplayingIndex > model.TargetIndex ? -1 : 1;
            ToNext();
        });
    }

    public void Deactivate()
    {
        animationSequence?.Kill();
    }

    private float GetPosition(int index)
    {
        return 1200 * index;
    }
}

public interface ICampaignMenuState
{
    public void Activate();
    public void Deactivate();
}  
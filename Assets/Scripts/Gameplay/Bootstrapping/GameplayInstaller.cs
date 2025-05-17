using System.Collections.Generic;
using Gameplay.AI;
using Gameplay.Execution;
using Gameplay.Execution.Builder;
using Gameplay.Execution.Dispatcher;
using Gameplay.Execution.Dispatcher.Systems;
using Gameplay.Execution.Engine;
using Gameplay.MoveGeneration.Generators;
using Gameplay.Player;
using Gameplay.Presentation;
using Gameplay.Presentation.Pieces;
using Gameplay.Presentation.UI;
using Gameplay.States;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Bootstrapping
{
    /// <summary>
    /// Installs and binds all dependencies and services required for gameplay using Zenject.
    /// </summary>
    public class GameplayInstaller : MonoInstaller
    {
        [FormerlySerializedAs("gamePresenterContainer")] [SerializeField]
        private GameViewContainer gameViewContainer;

        [SerializeField] private BoardView boardView;
        [SerializeField] private PromotionMenu promotionMenu;
        [SerializeField] private OpponentUIView opponentUI;
        [SerializeField] private OpponentConfig opponentConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(gameViewContainer);
            Container.BindInstance(boardView);
            Container.BindInstance(promotionMenu);
            Container.BindInstance(opponentConfig);

            Container.BindInterfacesAndSelfTo<OpponentUIPresenter>().AsSingle().WithArguments(opponentUI, new OpponentUIModel());
            Container.BindInterfacesAndSelfTo<MessageBoxPresenterPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayContext>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePresentation>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerTurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<AITurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChessAi>().AsSingle();
            Container.BindInterfacesAndSelfTo<BurstMoveFinder>().AsSingle();

            Container.BindInterfacesAndSelfTo<MoveSelectionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<DebugStepReactionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<AIDebugReactionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePresentationStepReactionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PreviewStepRectionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StepObserverDispatcherBuilder>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayExecutionEngineBuilder>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExecutionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExecutionEngineContextPool>().AsSingle();

            Container.BindFactory<IEnumerable<IGameplayStepReactionSystem>, StepObserverDispatcher, StepObserverDispatcher.Factory>().AsSingle();
            Container.BindFactory<StepObserverDispatcher, GameplayExecutionEngine, GameplayExecutionEngine.Factory>().AsSingle();
            Container.BindFactory<IPieceView, IPieceVisualModel, PiecePresenter, PiecePresenter.Factory>().AsSingle();
            Container.BindFactory<MessageBoxView, MessageBoxModel, MessageBoxPresenter, MessageBoxPresenter.Factory>().AsSingle();

            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle();
        }
    }
}
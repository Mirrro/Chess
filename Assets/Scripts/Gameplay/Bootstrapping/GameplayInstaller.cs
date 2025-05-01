using System.Collections.Generic;
using Gameplay.AI;
using Gameplay.Execution;
using Gameplay.Execution.Builder;
using Gameplay.Execution.Dispatcher;
using Gameplay.Execution.Dispatcher.Systems;
using Gameplay.Execution.Engine;
using Gameplay.Execution.Models;
using Gameplay.Execution.Moves;
using Gameplay.Player;
using Gameplay.Presentation;
using Gameplay.Presentation.Pieces;
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

        public override void InstallBindings()
        {
            Container.BindInstance(gameViewContainer);
            Container.BindInstance(boardView);
            Container.BindInstance(promotionMenu);

            Container.BindInterfacesAndSelfTo<GameplayContext>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePresentation>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerTurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<AITurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChessAi>().AsSingle();

            Container.BindInterfacesAndSelfTo<MoveSelectionService>().AsSingle();

            Container.BindInterfacesAndSelfTo<DebugStepReactionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GamePresentationStepReactionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PreviewStepRectionSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StepObserverDispatcherBuilder>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayExecutionEngineBuilder>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExecutionService>().AsSingle();

            Container.BindFactory<IEnumerable<IGameplayStepReactionSystem>, StepObserverDispatcher, StepObserverDispatcher.Factory>().AsSingle();
            Container.BindFactory<GameplayStateModel, IGameplayMove, StepObserverDispatcher, ExecutionEngineContext, ExecutionEngineContext.Factory>().AsSingle();
            Container.BindFactory<StepObserverDispatcher, GameplayExecutionEngine, GameplayExecutionEngine.Factory>().AsSingle();
            Container.BindFactory<IPieceView, IPieceVisualModel, PiecePresenter, PiecePresenter.Factory>().AsSingle();

            Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle();
        }
    }
}
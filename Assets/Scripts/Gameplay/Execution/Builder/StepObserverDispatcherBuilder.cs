using Gameplay.Execution.Dispatcher;
using Gameplay.Execution.Dispatcher.Systems;

namespace Gameplay.Execution.Builder
{
    /// <summary>
    /// Creates dispatchers pre-configured with relevant step reaction systems for live, preview, and AI modes.
    /// </summary>
    public class StepObserverDispatcherBuilder
    {
        private readonly StepObserverDispatcher.Factory factory;

        private readonly PreviewStepRectionSystem previewStepReactionSystem;
        private readonly GamePresentationStepReactionSystem gamePresentationStepReactionSystem;
        private readonly DebugStepReactionSystem debugStepReactionSystem;
        private readonly AIDebugReactionSystem aiDebugReactionSystem;

        public StepObserverDispatcherBuilder(StepObserverDispatcher.Factory factory,
            PreviewStepRectionSystem previewStepReactionSystem,
            GamePresentationStepReactionSystem gamePresentationStepReactionSystem,
            DebugStepReactionSystem debugStepReactionSystem, AIDebugReactionSystem aiDebugReactionSystem)
        {
            this.factory = factory;
            this.previewStepReactionSystem = previewStepReactionSystem;
            this.gamePresentationStepReactionSystem = gamePresentationStepReactionSystem;
            this.debugStepReactionSystem = debugStepReactionSystem;
            this.aiDebugReactionSystem = aiDebugReactionSystem;
        }

        public StepObserverDispatcher BuildLive()
        {
            return factory.Create(new IGameplayStepReactionSystem[]
                {debugStepReactionSystem, gamePresentationStepReactionSystem});
        }

        public StepObserverDispatcher BuildPreview()
        {
            return factory.Create(
                new IGameplayStepReactionSystem[] {previewStepReactionSystem});
        }

        public StepObserverDispatcher BuildAI()
        {
            return factory.Create(new IGameplayStepReactionSystem[] {});
        }
    }
}
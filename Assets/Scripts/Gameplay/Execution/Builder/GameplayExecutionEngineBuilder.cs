using Gameplay.Execution.Engine;

namespace Gameplay.Execution.Builder
{
    /// <summary>
    /// Builds execution engines with their respective step dispatcher configurations (Live, Preview, AI).
    /// </summary>
    public class GameplayExecutionEngineBuilder
    {
        private readonly GameplayExecutionEngine.Factory factory;
        private readonly StepObserverDispatcherBuilder stepObserverDispatcherBuilder;

        public GameplayExecutionEngineBuilder(GameplayExecutionEngine.Factory factory,
            StepObserverDispatcherBuilder stepObserverDispatcherBuilder)
        {
            this.factory = factory;
            this.stepObserverDispatcherBuilder = stepObserverDispatcherBuilder;
        }

        public GameplayExecutionEngine BuildLive()
        {
            return factory.Create(stepObserverDispatcherBuilder.BuildLive());
        }

        public GameplayExecutionEngine BuildPreview()
        {
            return factory.Create(stepObserverDispatcherBuilder.BuildPreview());
        }

        public GameplayExecutionEngine BuildAI()
        {
            return factory.Create(stepObserverDispatcherBuilder.BuildAI());
        }
    }
}
namespace Pipelines.Builder.Interfaces;

public interface IDispatcherBuilder
{
    public IPipelineBehaviorsBuilder AddDispatcher<TDispatcher>() where TDispatcher : class;
}
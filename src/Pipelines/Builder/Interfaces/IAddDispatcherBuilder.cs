namespace Pipelines.Builder.Interfaces;

public interface IAddDispatcherBuilder
{
    public IAddPipelineBehaviorsBuilder AddDispatcher<TDispatcher>();
}
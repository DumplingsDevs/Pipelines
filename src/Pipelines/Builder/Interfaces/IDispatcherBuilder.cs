namespace Pipelines.Builder.Interfaces;

public interface IDispatcherBuilder
{
    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>() where TDispatcher : class;
    public IPipelineDecoratorBuilder AddDispatcher<TDispatcher>(DispatcherOptions options) where TDispatcher : class;
}
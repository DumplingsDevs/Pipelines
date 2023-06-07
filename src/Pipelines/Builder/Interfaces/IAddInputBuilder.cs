namespace Pipelines.Builder.Interfaces;

public interface IAddInputBuilder
{
    public IAddHandlerBuilder AddInput<TInputType>();
}
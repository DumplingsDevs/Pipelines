namespace Pipelines.Builder.Interfaces;

public interface IInputBuilder
{
    public IHandlerBuilder AddInput(Type type);
}
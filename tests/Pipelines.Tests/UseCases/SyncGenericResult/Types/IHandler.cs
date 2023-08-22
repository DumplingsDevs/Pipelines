namespace Pipelines.Tests.UseCases.SyncGenericResult.Types;

public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult: class
{
    public TResult Handle(TCommand command);
}
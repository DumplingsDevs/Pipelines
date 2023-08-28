namespace Pipelines.Tests.UseCases.SyncGenericResult.Types;

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult: class
{
    public TResult Handle(TInput command);
}
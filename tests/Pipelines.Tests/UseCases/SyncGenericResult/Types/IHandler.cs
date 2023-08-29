namespace Pipelines.Tests.UseCases.SyncGenericResult.Types;

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult>
{
    public TResult Handle(TInput input);
}
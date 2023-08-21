namespace Pipelines.Tests.UseCases.SyncGenericResult.Types;

public interface IDispatcher
{
    public TResult Send<TResult>(IInput<TResult> input) where TResult : class;
}
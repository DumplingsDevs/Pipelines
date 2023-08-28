namespace Pipelines.Tests.SharedLibraryTest.Types;

public interface IHandlerShared<in TInput, TResult> where TInput : IInputShared<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
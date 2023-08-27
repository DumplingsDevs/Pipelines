namespace Pipelines.Tests.SharedLibraryTest.Types;

public interface IHandlerShared<in TCommand, TResult> where TCommand : IInputShared<TResult> where TResult: class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
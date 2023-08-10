using Pipelines.Tests.Models;

namespace Pipelines.Tests.UseCases.HandlerWithInputFromDifferentLibrary.Types;

public interface ICommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken token) where TResult : class;
}
using Pipelines.Tests.Models;

namespace Pipelines.Tests.UseCases.HandlerWithInputFromDifferentLibrary.Types;

public interface ICommandHandler<in TInput, TResult> where TInput : ICommand<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
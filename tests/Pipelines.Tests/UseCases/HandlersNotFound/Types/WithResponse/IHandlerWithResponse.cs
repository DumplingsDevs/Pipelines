using Pipelines.Tests.UseCases.HandlerWithResult.Types;

namespace Pipelines.Tests.UseCases.HandlersNotFound.Types.WithResponse;

public interface IHandlerWithResponse<in TInput, TResult> where TInput : IInputWithResponse<TResult>
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
namespace Pipelines.Tests.UseCases.CreateDIScope.Types.WithResponse;

public interface IHandlerWithResponse<in TInput, TResult> where TInput : IInputWithResponse<TResult>
{
    public Task<TResult> HandleAsync(TInput input, CancellationToken token);
}
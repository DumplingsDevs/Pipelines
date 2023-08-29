namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskInputGenericResult<in TInput, TResult> where TInput : IInputGenericType<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
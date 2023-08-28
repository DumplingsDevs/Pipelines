namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskInputGenericResult<in TInput, TResult> where TInput : IInputGenericType<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
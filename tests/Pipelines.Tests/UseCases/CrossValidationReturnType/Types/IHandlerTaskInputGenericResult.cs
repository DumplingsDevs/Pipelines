namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskInputGenericResult<in TCommand, TResult> where TCommand : IInputGenericType<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
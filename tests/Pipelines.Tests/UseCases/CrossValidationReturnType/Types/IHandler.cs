namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult : class
{
    public TResult HandleAsync(TCommand command, CancellationToken token);
}
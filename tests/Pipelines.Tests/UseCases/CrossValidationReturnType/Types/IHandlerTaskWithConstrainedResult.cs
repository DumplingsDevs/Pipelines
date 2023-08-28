namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskWithConstrainedResult<in TInput, TResultOne> where TInput : IInputType
    where TResultOne : IResultOne
{
    public Task<TResultOne> HandleAsync(TInput command, CancellationToken token);
}
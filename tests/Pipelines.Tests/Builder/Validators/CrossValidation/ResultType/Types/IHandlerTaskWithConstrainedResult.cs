namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerTaskWithConstrainedResult<in TCommand, TResultOne> where TCommand : IInputType
    where TResultOne : IResultOne
{
    public Task<TResultOne> HandleAsync(TCommand command, CancellationToken token);
}
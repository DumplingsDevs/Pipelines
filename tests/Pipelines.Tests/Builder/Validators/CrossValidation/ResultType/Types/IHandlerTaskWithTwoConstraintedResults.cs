namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerTaskWithTwoConstraintedResults<in TCommand, TResultOne, TResultTwo> where TCommand : IInputType
    where TResultOne : IResultOne
    where TResultTwo : IResultTwo
{
    public Task<(TResultOne,TResultTwo)> HandleAsync(TCommand command, CancellationToken token);
}
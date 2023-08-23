namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskWithTwoConstraintedResults<in TCommand, TResultOne, TResultTwo> where TCommand : IInputType
    where TResultOne : class, IResultOne
    where TResultTwo : class, IResultTwo
{
    public Task<(TResultOne,TResultTwo)> HandleAsync(TCommand command, CancellationToken token);
}
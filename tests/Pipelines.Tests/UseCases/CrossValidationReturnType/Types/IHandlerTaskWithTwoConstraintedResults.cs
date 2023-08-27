namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskWithTwoConstraintedResults<in TCommand, TResultOne, TResultTwo> where TCommand : IInputType
{
    public Task<(TResultOne,TResultTwo)> HandleAsync(TCommand command, CancellationToken token);
}
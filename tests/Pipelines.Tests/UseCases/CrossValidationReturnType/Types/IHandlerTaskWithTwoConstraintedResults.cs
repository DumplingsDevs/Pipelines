namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IHandlerTaskWithTwoConstraintedResults<in TInput, TResultOne, TResultTwo> where TInput : IInputType
    where TResultOne : IResultOne
    where TResultTwo : IResultTwo
{
    public Task<(TResultOne, TResultTwo)> HandleAsync(TInput command, CancellationToken token);
}
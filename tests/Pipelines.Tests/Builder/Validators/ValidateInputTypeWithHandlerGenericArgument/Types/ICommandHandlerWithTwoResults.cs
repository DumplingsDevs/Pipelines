namespace Pipelines.Tests.Builder.Validators.ValidateInputTypeWithHandlerGenericArgument.Types;

public interface ICommandHandlerWithTwoResults<in TCommand, TResult, TSecondResult> where TCommand : ICommandWithTwoResults<TResult, TSecondResult>
{
    public Task<(TResult,TSecondResult)> HandleAsync(TCommand command, CancellationToken token);
}
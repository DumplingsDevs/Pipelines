using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Invalid;

public interface IReturnTwoValuesExpectedOne<in TCommand, TResult> where TCommand : ICommandWithResult<TResult>
{
    public Task<(TResult,bool)> HandleAsync(TCommand command, CancellationToken token);
}
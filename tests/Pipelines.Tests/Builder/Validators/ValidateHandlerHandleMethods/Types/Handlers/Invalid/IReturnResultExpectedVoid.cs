using Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.ValidateHandlerHandleMethods.Types.Handlers.Invalid;

public interface IReturnResultExpectedVoid<in TCommand, TResult> where TCommand : ICommand
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
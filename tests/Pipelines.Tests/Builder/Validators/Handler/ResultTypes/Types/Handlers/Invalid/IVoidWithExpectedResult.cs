using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IVoidWithExpectedResult<in TCommand, TResult> where TCommand : ICommandWithResult<int>
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}
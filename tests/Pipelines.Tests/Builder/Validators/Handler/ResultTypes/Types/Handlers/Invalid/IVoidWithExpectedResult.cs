using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IVoidWithExpectedResult<in TCommand> where TCommand : ICommandWithResult<int>
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}
using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnResultExpectedVoid<in TCommand> where TCommand : ICommand
{
    public Task<int> HandleAsync(TCommand command, CancellationToken token);
}
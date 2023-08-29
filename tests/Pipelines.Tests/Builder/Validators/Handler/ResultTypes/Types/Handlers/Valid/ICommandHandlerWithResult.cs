using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;

public interface ICommandHandlerWithResult<in TInput, TResult>
    where TInput : IInputWithResult<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Valid;

public interface IVoidCommandHandler<in TInput> where TInput : ICommand
{
    public Task HandleAsync(TInput command, CancellationToken token);
}
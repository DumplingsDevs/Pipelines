using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IReturnResultTypeNotSpecifiedInGenerics<in TInput> where TInput : IInput
{
    public Task<int> HandleAsync(TInput command, CancellationToken token);
}
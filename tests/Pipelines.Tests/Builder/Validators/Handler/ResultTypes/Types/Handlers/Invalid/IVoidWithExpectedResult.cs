using Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.InputTypes;

namespace Pipelines.Tests.Builder.Validators.Handler.ResultTypes.Types.Handlers.Invalid;

public interface IVoidWithExpectedResult<in TInput, TResult> where TInput : IInputWithResult<int>
{
    public Task HandleAsync(TInput input, CancellationToken token);
}
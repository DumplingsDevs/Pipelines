namespace Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod.Types;

public interface ISingleMethod<in TInput, TResult> where TInput : ICommand<TResult>
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
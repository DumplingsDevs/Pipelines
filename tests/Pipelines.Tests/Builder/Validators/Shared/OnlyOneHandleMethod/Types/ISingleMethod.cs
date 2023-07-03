namespace Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod.Types;

public interface ISingleMethod<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
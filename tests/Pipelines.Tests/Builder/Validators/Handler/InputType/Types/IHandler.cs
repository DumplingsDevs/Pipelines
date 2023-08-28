namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface IHandler<in TInput> where TInput : IInput
{
    public Task HandleAsync(TInput command, CancellationToken token);
}
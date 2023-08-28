namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface ICommandHandler<in TInput> where TInput : ICommand
{
    public Task HandleAsync(TInput command, CancellationToken token);
}
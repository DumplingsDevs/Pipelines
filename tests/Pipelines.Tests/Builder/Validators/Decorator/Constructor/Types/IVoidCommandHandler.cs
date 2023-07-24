namespace Pipelines.Tests.Builder.Validators.Decorator.Constructor.Types;

public interface IVoidCommandHandler<in TCommand> where TCommand : IVoidCommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}
namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface.Types;

public interface IVoidCommandHandler<in TCommand> where TCommand : IVoidCommand
{
    public Task HandleAsync(TCommand command, CancellationToken token);
}
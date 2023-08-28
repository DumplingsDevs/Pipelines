namespace Pipelines.Tests.Builder.Validators.Decorator.ImplementsInterface.Types;

public interface IVoidCommandHandler<in TInput> where TInput : IVoidCommand
{
    public Task HandleAsync(TInput command, CancellationToken token);
}
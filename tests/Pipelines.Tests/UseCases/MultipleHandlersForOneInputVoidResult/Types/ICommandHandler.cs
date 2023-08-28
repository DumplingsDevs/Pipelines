namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Types;

public interface ICommandHandler<in TInput> where TInput : ICommand
{
    public void HandleAsync(TInput command, CancellationToken token);
}
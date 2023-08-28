namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Types;

public interface ICommandHandler<in TInput> where TInput : ICommand
{
    public Task HandleAsync(TInput command, CancellationToken token);
}
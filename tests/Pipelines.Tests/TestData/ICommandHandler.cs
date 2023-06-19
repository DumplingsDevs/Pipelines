namespace Pipelines.Tests.TestData;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<string> HandleAsync(TCommand command);
}
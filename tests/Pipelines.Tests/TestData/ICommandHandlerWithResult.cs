namespace Pipelines.Tests.TestData;

public interface ICommandHandlerWithResult<in TCommand> where TCommand : ICommandWithResult
{
    public Task<string> HandleAsync(TCommand command);
}
namespace Pipelines.Tests.TestData;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    public Task<string> HandleAsync(ExampleCommand command)
    {
        return Task.FromResult($"It's working!, {command.Name}, {command.Value}");
    }
}
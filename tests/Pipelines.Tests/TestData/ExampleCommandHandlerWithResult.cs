namespace Pipelines.Tests.TestData;

public class ExampleCommandHandlerWithResult : ICommandHandlerWithResult<ExampleCommandWithResult>
{
    public Task<string> HandleAsync(ExampleCommandWithResult commandWithResult)
    {
        return Task.FromResult($"It's working!, {commandWithResult.Name}, {commandWithResult.Value}");
    }
}
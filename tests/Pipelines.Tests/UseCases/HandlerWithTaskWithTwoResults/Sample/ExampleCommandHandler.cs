namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTwoResults.Sample;
using Types;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult, int>
{
    public Task<(ExampleCommandResult, int)> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult((new ExampleCommandResult(command.Value), 5));
    }
}
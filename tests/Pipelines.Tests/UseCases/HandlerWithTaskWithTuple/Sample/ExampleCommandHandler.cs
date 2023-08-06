using Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult, ExampleCommandResultSecond>
{
    public Task<(ExampleCommandResult, ExampleCommandResultSecond)> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult((new ExampleCommandResult(command.Value), new ExampleCommandResultSecond("Value")));
    }
}
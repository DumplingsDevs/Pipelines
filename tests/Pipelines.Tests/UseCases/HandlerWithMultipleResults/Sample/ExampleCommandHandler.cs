namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Sample;
using Types;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleRecordCommandResult, ExampleCommandClassResult>
{
    public (ExampleRecordCommandResult, ExampleCommandClassResult) HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return (new ExampleRecordCommandResult(command.Value), new ExampleCommandClassResult("Value"));
    }
}
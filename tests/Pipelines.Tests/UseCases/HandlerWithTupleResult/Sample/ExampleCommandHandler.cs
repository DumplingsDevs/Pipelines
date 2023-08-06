using Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleRecordCommandResult, ExampleCommandClassResult>
{
    public (ExampleRecordCommandResult, ExampleCommandClassResult) HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return (new ExampleRecordCommandResult(command.Value), new ExampleCommandClassResult("Value"));
    }
}
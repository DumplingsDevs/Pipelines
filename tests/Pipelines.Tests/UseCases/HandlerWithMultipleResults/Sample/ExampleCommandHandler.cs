namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Sample;
using Types;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult, int>
{
    public (ExampleCommandResult, int) HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return (new ExampleCommandResult(command.Value), 5);
    }
}
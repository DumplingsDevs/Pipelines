using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleCommand command, CancellationToken token, bool canDoSomething, Dictionary<string, string> dictionary)
    {
        return Task.FromResult(new ExampleCommandResult(command.Value));
    }
}
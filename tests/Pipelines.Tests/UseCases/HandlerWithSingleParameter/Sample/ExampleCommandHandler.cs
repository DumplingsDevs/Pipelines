using Pipelines.Tests.UseCases.HandlerWithSingleParameter.Types;

namespace Pipelines.Tests.UseCases.HandlerWithSingleParameter.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleCommand command)
    {
        return Task.FromResult(new ExampleCommandResult(command.Value));
    }
}
using Pipelines.Tests.UseCases.HandlerWithResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResult.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(command.Value));
    }
    
    public Task<ExampleCommandResult> HandleAsync1(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(command.Value));
    }
}
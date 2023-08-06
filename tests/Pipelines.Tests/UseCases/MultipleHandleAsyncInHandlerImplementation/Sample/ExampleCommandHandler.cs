using Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Types;

namespace Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(CancellationToken token, ExampleCommand command)
    {
        return null!;
    }
    
    public Task<ExampleCommandResult> HandleAsync(ExampleCommand command)
    {
        return null!;
    }

    public Task<ExampleCommandResult> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(command.Value));
    }
}
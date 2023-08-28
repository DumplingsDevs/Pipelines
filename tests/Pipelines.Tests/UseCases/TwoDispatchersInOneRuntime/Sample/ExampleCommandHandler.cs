using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Commands;

namespace Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime.Sample;

internal class ExampleCommandHandler : ICommandHandler<ExampleCommand, CommandResult>
{
    public Task<CommandResult> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult(new CommandResult(command.Value));
    }
}

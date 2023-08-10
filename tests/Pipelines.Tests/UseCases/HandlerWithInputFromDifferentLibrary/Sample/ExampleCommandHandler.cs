using Pipelines.Tests.Models;
using Pipelines.Tests.UseCases.HandlerWithInputFromDifferentLibrary.Types;

namespace Pipelines.Tests.UseCases.HandlerWithInputFromDifferentLibrary.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(command.Value));
    }
}
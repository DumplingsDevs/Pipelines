using Pipelines.Tests.UseCases.TaskVoidHandler.Types;

namespace Pipelines.Tests.UseCases.TaskVoidHandler.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    public Task HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}
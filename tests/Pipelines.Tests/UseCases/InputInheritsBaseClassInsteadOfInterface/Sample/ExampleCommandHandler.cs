using Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Types;

namespace Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    public Task HandleAsync(ExampleCommand command, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}
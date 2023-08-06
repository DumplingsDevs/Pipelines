using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    public void HandleAsync(ExampleCommand command, CancellationToken token)
    { }
}
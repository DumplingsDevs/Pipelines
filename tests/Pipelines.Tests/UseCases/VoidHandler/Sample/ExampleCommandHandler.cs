using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleCommandHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public void HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _decoratorsState.Status.Add(nameof(ExampleCommandHandler));
    }
}
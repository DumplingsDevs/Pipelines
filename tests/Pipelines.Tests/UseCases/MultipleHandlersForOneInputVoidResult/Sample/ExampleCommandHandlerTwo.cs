namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Sample;
using Types;

public class ExampleCommandHandlerTwo : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _state;

    public ExampleCommandHandlerTwo(DecoratorsState state)
    {
        _state = state;
    }

    public void HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleCommandHandlerTwo));
    }
}
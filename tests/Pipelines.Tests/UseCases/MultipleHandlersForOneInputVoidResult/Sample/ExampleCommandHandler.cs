namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidResult.Sample;
using Types;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _state;

    public ExampleCommandHandler(DecoratorsState state)
    {
        _state = state;
    }

    public void HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleCommandHandler));
    }
}
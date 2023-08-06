using Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Types;

namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _state;

    public ExampleCommandHandler(DecoratorsState state)
    {
        _state = state;
    }

    public Task HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleCommandHandler));
        return Task.CompletedTask;
    }
}
using Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Types;

namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Sample;

public class ExampleCommandHandlerTwo : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _state;

    public ExampleCommandHandlerTwo(DecoratorsState state)
    {
        _state = state;
    }

    public Task HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleCommandHandlerTwo));

        return Task.CompletedTask;
    }
}
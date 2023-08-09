namespace Pipelines.Tests.UseCases.TaskVoidHandler.Sample;
using Types;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleCommandHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public Task HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _decoratorsState.Status.Add(nameof(ExampleCommandHandler));
        return Task.CompletedTask;
    }
}
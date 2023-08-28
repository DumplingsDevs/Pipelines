namespace Pipelines.Tests.UseCases.InputWithInheritanceDispatcherWithGenericInput.Samples;
using Types;

public class ExampleHandler : IHandler<ExampleInput>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public Task HandleAsync(ExampleInput input, CancellationToken token)
    {
        _decoratorsState.Status.Add(nameof(ExampleHandler));
        return Task.CompletedTask;
    }
}
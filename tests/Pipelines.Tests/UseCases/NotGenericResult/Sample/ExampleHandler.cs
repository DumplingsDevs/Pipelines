using Pipelines.Tests.UseCases.NotGenericResult.Types;

namespace Pipelines.Tests.UseCases.NotGenericResult.Sample;

public class ExampleHandler : IHandler<ExampleInput>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public Task<string> HandleAsync(ExampleInput input, CancellationToken token)
    {
        _decoratorsState.Status.Add(nameof(ExampleHandler));
        return Task.FromResult($"It's working!, {input.Name}, {input.Value}");
    }
}
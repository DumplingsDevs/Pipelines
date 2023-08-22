namespace Pipelines.Tests.UseCases.SyncNotGenericResult.Sample;
using Types;

public class ExampleHandler : IHandler<ExampleInput>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public string Handle(ExampleInput input)
    {
        _decoratorsState.Status.Add(nameof(ExampleHandler));
        return $"It's working!, {input.Name}, {input.Value}";
    }
}
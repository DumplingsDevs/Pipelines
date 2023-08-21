using Pipelines.Tests.UseCases.VoidHandler.Types;

namespace Pipelines.Tests.UseCases.VoidHandler.Sample;

public class ExampleHandler : IHandler<ExampleInput>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public void Handle(ExampleInput input)
    {
        _decoratorsState.Status.Add(nameof(ExampleHandler));
    }
}
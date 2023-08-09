using Pipelines.Tests.UseCases.NotGenericResult.Types;

namespace Pipelines.Tests.UseCases.NotGenericResult.Sample;

public class ExampleCommandHandler : ICommandHandler<ExampleCommand>
{
    private readonly DecoratorsState _decoratorsState;

    public ExampleCommandHandler(DecoratorsState decoratorsState)
    {
        _decoratorsState = decoratorsState;
    }

    public Task<string> HandleAsync(ExampleCommand command, CancellationToken token)
    {
        _decoratorsState.Status.Add(nameof(ExampleCommandHandler));
        return Task.FromResult($"It's working!, {command.Name}, {command.Value}");
    }
}
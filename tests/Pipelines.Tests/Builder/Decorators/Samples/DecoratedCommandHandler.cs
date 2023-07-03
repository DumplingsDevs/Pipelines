using Pipelines.Tests.Builder.Decorators.Types;

namespace Pipelines.Tests.Builder.Decorators.Samples;

public class DecoratedCommandHandler : ICommandHandler<DecoratedCommand, DecoratedCommandResult>
{
    public Task<DecoratedCommandResult> HandleAsync(DecoratedCommand command, CancellationToken token)
    {
        return Task.FromResult(new DecoratedCommandResult());
    }
}
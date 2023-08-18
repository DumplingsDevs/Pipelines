using Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithTuple.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult, ExampleCommandResultSecond>
{
    public Task<(ExampleCommandResult, ExampleCommandResultSecond)> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult((new ExampleCommandResult(input.Value), new ExampleCommandResultSecond("Value")));
    }
}
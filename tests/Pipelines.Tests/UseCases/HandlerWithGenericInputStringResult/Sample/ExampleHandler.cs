using Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithGenericInputStringResult.Sample;

public class ExampleHandler : IHandler<ExampleInput, string>
{
    public Task<string> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(input.Value);
    }
}
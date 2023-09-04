using Pipelines.Tests.UseCases.HandlerWithStructResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithStructResult.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
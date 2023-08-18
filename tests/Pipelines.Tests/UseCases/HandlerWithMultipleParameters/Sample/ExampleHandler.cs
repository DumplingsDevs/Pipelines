using Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token, bool canDoSomething, Dictionary<string, string> fancyDictionary)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
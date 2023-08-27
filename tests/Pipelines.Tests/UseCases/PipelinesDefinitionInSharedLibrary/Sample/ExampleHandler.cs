using Pipelines.Tests.SharedLibraryTest.Types;

namespace Pipelines.Tests.UseCases.PipelinesDefinitionInSharedLibrary.Sample;

public class ExampleHandler : IHandlerShared<ExampleInput, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
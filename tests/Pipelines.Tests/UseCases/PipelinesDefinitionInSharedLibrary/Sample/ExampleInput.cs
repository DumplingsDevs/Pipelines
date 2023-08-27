
using Pipelines.Tests.SharedLibraryTest.Types;

namespace Pipelines.Tests.UseCases.PipelinesDefinitionInSharedLibrary.Sample;

public record ExampleInput(string Value) : IInputShared<ExampleCommandResult>;
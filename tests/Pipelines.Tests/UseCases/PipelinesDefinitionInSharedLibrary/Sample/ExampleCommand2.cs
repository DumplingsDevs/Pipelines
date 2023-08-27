
using Pipelines.Tests.SharedLibraryTest.Types;

namespace Pipelines.Tests.UseCases.PipelinesDefinitionInSharedLibrary.Sample;

public record ExampleCommand2(string Value) : IInputShared<ExampleCommandResult>;
using Pipelines.Tests.UseCases.HandlerWithBigTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithBigTuple.Sample;

public record ExampleInput(string Value) : IInput<ExampleCommandResult, ExampleCommandResult2, ExampleCommandResult3,
    ExampleCommandResult4, ExampleCommandResult5, ExampleCommandResult6, ExampleCommandResult7>;
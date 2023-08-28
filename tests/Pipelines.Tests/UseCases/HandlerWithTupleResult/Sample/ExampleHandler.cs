using Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleRecordCommandResult, ExampleCommandClassResult>
{
    public (ExampleRecordCommandResult, ExampleCommandClassResult) HandleAsync(ExampleInput input)
    {
        return (new ExampleRecordCommandResult(input.Value), new ExampleCommandClassResult("Value"));
    }
}
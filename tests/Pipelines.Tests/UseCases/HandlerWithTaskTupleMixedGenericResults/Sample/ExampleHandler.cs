using Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleRecordCommandResult, ExampleCommandClassResult, ExampleCommandClassResult2>
{
    public (ExampleRecordCommandResult, ExampleCommandClassResult, ExampleCommandClassResult2) HandleAsync(ExampleInput input,
        CancellationToken token)
    {
        return (new ExampleRecordCommandResult(input.Value), new ExampleCommandClassResult("Value"), new ExampleCommandClassResult2("Value 2"));
    }
}
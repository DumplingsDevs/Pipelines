using System.Drawing;
using Pipelines.Tests.UseCases.HandlerWithTaskWithBigTuple.Types;

namespace Pipelines.Tests.UseCases.HandlerWithTaskWithBigTuple.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult, ExampleCommandResult2, ExampleCommandResult3,
    ExampleCommandResult4, ExampleCommandResult5, ExampleCommandResult6, ExampleCommandResult7>
{
    public Task<(ExampleCommandResult, ExampleCommandResult2, ExampleCommandResult3, ExampleCommandResult4,
        ExampleCommandResult5, ExampleCommandResult6, ExampleCommandResult7)> HandleAsync(ExampleInput input,
        CancellationToken token)
    {
        return Task.FromResult((new ExampleCommandResult(input.Value),
            new ExampleCommandResult2("Value"), new ExampleCommandResult3(1), new ExampleCommandResult4(21.37),
            new ExampleCommandResult5(new Rectangle(10, 10, 10, 10)), new ExampleCommandResult6(DateTime.Parse("2023-05-06 22:30:00.531")),
            new ExampleCommandResult7("Value2")));
    }
}
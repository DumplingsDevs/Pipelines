namespace Pipelines.Tests.UseCases.SyncGenericResult.Sample;
using Types;

public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>
{
    public ExampleCommandResult Handle(ExampleInput input)
    {
        return new ExampleCommandResult(input.Value);
    }
}
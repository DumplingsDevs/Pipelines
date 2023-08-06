namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Sample;

public class ExampleCommandClassResult
{
    public ExampleCommandClassResult(string value)
    {
        Value = value;
    }

    public string Value { get; }
}


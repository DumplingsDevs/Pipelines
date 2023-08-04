namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Sample;

public class ExampleCommandClassResult
{
    public ExampleCommandClassResult(string value)
    {
        Value = value;
    }

    public string Value { get; }
}


namespace Pipelines.Tests.UseCases.HandlerWithStructResult.Sample;

public struct ExampleCommandResult
{
    public ExampleCommandResult(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
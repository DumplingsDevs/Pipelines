namespace Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod.Types;

public interface IDifferentResultType
{
    public Task<string> HandleButDifferentName(HandleParameters handleParameters, CancellationToken token);
}
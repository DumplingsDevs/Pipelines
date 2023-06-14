namespace Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod.Types;

public interface IHandleMethodButDifferentName
{
    public Task HandleButDifferentName(HandleParameters handleParameters, CancellationToken token);
}
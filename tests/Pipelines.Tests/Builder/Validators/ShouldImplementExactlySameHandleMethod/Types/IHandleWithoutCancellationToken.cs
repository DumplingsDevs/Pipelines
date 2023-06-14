namespace Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod.Types;

public interface IHandleWithoutCancellationToken
{
    public Task Handle(HandleParameters handleParameters);
}
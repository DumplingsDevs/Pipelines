namespace Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod.Types;

public interface IHandleMethod
{
    public Task Handle(HandleParameters handleParameters, CancellationToken token);
}
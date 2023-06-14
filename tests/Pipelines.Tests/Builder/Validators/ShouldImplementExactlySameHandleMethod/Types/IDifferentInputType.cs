namespace Pipelines.Tests.Builder.Validators.ShouldImplementExactlySameHandleMethod.Types;

public interface IDifferentInputType
{
    public Task Handle(int handleParameters, CancellationToken token);
}
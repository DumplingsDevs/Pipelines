namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface IHandlerWithoutGenericArgumentsConstraint<in TInput>
{
    public Task HandleAsync(TInput input, CancellationToken token);
}
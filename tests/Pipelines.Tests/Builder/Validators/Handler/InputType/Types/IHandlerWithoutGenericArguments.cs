namespace Pipelines.Tests.Builder.Validators.Handler.InputType.Types;

public interface IHandlerWithoutGenericArguments
{
    public Task HandleAsync(IInput command, CancellationToken token);
}
namespace Pipelines.Tests.Builder.Validators.Dispatcher.InputType.Types;

public interface IDispatcher
{
    public Task SendAsync(IInput request, CancellationToken token);
}
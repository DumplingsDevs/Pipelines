namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IVoidDispatcher
{
    public Task SendAsync(IInput request, CancellationToken token);
}
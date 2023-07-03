namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IVoidDispatcher
{
    public Task SendAsync(ICommand request, CancellationToken token);
}
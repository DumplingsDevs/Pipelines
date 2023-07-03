namespace Pipelines.Tests.Builder.Validators.Dispatcher.InputType.Types;

public interface IDispatcher
{
    public Task SendAsync(ICommand request, CancellationToken token);
}
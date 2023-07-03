namespace Pipelines.Tests.Builder.Validators.Dispatcher.InputType.Types;

public interface IDispatcherWithIncorrectParameter
{
    public Task SendAsync(int request, CancellationToken token);
}
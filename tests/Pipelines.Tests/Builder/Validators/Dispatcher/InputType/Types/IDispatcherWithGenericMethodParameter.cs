namespace Pipelines.Tests.Builder.Validators.Dispatcher.InputType.Types;

public interface IDispatcherWithGenericMethodParameter
{
    Task SendAsync<TInput>(TInput command, CancellationToken cancellationToken = default)
        where TInput : class, ICommand;
}
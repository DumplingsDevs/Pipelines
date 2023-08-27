namespace Pipelines.Tests.Builder.Validators.Dispatcher.ResultTypes.Types;

public interface IDispatcherWithGenericInput
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand;
}
namespace Pipelines.Tests.UseCases.InputInheritsBaseClassInsteadOfInterface.Types;

public interface ICommandDispatcher
{
    public Task SendAsync(ICommand command, CancellationToken token);
}
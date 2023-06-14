namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

public interface IDecoratorCommandDispatcher
{
    public Task<TResult> SendAsync<TResult>(IDecoratorCommand<TResult> decoratorCommand, CancellationToken token);
}
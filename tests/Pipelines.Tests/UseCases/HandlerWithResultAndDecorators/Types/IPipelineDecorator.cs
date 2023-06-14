namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

public interface IPipelineDecorator<T>
{
    public Task<TResult> HandleAsync<TCommand, TResult>(TCommand command,  CancellationToken token);
}
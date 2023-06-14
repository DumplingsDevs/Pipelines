namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

public interface IDecoratorCommandHandler<in TCommand, TResult> where TCommand : IDecoratorCommand<TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
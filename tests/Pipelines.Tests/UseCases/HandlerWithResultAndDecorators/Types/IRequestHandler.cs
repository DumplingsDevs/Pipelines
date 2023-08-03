namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TRequest request, CancellationToken token);
}
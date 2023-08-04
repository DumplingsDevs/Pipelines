namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

public interface IRequestDispatcher
{
    public Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken token) where TResult : class;
}
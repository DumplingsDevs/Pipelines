namespace Pipelines.Tests.Builder.Validators.Shared.CompareTypes.Types;

public interface IQueryDispatcher
{
    public Task<TResult> Handle<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        where TResult : class;
}
namespace Pipelines.Tests.Builder.Validators.Shared.CompareTypes.Types;

public interface IQueryDispatcherWithNoGenericResult
{
    public Task<int> Handle<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
}
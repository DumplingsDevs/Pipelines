using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Queries;

namespace Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime.Sample;

internal class ExampleQueryHandler : IQueryHandler<ExampleQuery, List<QueryResult>>
{
    public Task<List<QueryResult>> HandleAsync(ExampleQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(Enumerable.Range(1, 10).Select(x => new QueryResult(query.Id + x)).ToList());
    }
}
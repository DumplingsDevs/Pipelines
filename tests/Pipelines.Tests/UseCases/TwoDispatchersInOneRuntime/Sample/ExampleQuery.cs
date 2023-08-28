using Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Queries;

namespace Pipelines.Tests.UseCases.TwoDispatchersInOneRuntime.Sample;

internal record ExampleQuery(string Id) : IQuery<List<QueryResult>>
{
    
}

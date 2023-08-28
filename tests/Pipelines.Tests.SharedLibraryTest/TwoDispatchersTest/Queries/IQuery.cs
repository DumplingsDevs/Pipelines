namespace Pipelines.Tests.SharedLibraryTest.TwoDispatchersTest.Queries;

// ReSharper disable once UnusedTypeParameter
// kbytner 08.05.2022 - rule disabled due to usage in QueryDispatcher
public interface IQuery<TQueryResult> where TQueryResult : class
{ }
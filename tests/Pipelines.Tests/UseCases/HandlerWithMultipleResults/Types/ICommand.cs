namespace Pipelines.Tests.UseCases.HandlerWithMultipleResults.Types;

public interface ICommand<TResult, TResult2> where TResult : class where TResult2 : class
{
}
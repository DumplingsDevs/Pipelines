namespace Pipelines.Tests.UseCases.HandlerWithTupleResult.Types;

public interface ICommand<TResult, TResult2> where TResult : class where TResult2 : class
{
}
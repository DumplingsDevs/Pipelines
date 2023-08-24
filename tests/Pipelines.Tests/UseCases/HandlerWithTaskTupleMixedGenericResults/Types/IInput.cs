namespace Pipelines.Tests.UseCases.HandlerWithTaskTupleMixedGenericResults.Types;

public interface IInput<TResult, TResult2> where TResult : class where TResult2 : class
{ }
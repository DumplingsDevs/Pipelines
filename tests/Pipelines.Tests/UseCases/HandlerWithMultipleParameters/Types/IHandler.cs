namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

public interface IHandler<in TInput, TResult> where TInput : IInput<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token, bool canDoSomething,
        Dictionary<string, string> fancyDictionary);
}
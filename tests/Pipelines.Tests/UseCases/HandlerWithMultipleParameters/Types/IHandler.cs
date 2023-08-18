namespace Pipelines.Tests.UseCases.HandlerWithMultipleParameters.Types;

public interface IHandler<in TCommand, TResult> where TCommand : IInput<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token, bool canDoSomething,
        Dictionary<string, string> fancyDictionary);
}
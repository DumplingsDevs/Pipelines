namespace Pipelines.Tests.UseCases.HandlerWithSingleParameter.Types;

public interface ICommandHandler<in TInput, TResult> where TInput : ICommand<TResult> where TResult : class
{
    public Task<TResult> HandleAsync(TInput command);
}
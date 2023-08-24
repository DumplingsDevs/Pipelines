namespace Pipelines.Tests.UseCases.HandlerWithTaskWithBigTuple.Types;

public interface IHandler<in TCommand,TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
    where TCommand : IInput<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7> 
    where TResult : class
    where TResult2 : class
    where TResult3 : class
    where TResult4 : class
    where TResult5 : class
    where TResult6 : class
    where TResult7 : class
{
    public Task<(TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7)> HandleAsync(TCommand command, CancellationToken token);
}
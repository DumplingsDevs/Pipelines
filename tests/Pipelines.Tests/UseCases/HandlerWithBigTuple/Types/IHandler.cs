namespace Pipelines.Tests.UseCases.HandlerWithBigTuple.Types;

public interface IHandler<in TCommand,TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
    where TCommand : IInput<TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>
{
    public (TResult, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7) HandleAsync(TCommand command, CancellationToken token);
}
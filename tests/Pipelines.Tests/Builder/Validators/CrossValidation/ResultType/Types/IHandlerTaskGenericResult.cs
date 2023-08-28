namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerTaskGenericResult<in TInput, TResult> where TInput : IInputType
{
    public Task<TResult> HandleAsync(TInput command, CancellationToken token);
}
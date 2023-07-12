namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerTaskGenericResult<in TCommand, TResult> where TCommand : IInputType
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken token);
}
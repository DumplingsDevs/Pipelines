namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerIntResult<in TInput> where TInput : IInputType
{
    public int HandleAsync(TInput command, CancellationToken token);
}
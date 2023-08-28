namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerStringResult<in TInput> where TInput : IInputType
{
    public string HandleAsync(TInput command, CancellationToken token);
}
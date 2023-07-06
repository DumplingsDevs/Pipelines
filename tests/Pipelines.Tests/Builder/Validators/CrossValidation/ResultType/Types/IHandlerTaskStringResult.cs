namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerTaskStringResult<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}
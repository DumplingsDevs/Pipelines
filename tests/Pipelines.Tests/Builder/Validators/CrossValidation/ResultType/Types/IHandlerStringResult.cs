namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IHandlerStringResult<in TCommand> where TCommand : IInputType
{
    public string HandleAsync(TCommand command, CancellationToken token);
}
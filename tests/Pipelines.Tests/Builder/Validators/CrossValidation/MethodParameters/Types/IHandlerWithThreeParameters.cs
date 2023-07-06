namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IHandlerWithThreeParameters<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command, int index, CancellationToken token);
}
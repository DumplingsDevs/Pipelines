namespace Pipelines.Tests.UseCases.CrossValidationMethodParameters.Types;

public interface IHandlerWithInputTypeOnly<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command);
}
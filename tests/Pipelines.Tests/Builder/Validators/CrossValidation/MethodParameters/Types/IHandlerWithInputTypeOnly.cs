namespace Pipelines.Tests.Builder.Validators.CrossValidation.MethodParameters.Types;

public interface IHandlerWithInputTypeOnly<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command);
}
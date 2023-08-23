namespace Pipelines.Tests.UseCases.InputValidation.Types;

public interface IHandlerWithWrongInputTypePosition<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(CancellationToken token, TCommand command);
}
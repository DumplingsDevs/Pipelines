namespace Pipelines.Tests.UseCases.TypeOfMissingValidation.Types;

public interface IHandler<in TCommand> where TCommand : IInputType
{
    public Task<string> HandleAsync(TCommand command, CancellationToken token);
}
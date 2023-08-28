namespace Pipelines.Tests.UseCases.TypeOfMissingValidation.Types;

public interface IHandler<in TInput> where TInput : IInputType
{
    public Task<string> HandleAsync(TInput command, CancellationToken token);
}
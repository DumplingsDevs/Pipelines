namespace Pipelines.Tests.UseCases.TypeOfMissingValidation.Types;

public interface IDispatcher
{
    public Task<string> SendAsync(IInputType inputType, CancellationToken token);
}
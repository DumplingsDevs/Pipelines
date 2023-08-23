namespace Pipelines.Tests.UseCases.InputValidation.Types;

public interface IDispatcherWithWrongInputTypePosition
{
    public Task<string> SendAsync(CancellationToken token, IInputType inputType);
}
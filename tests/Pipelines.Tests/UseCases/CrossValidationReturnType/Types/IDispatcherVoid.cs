namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherVoid
{
    public void SendAsync(IInputType inputType, CancellationToken token);
}
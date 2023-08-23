namespace Pipelines.Tests.UseCases.CrossValidationReturnType.Types;

public interface IDispatcherStringResult
{
    public string SendAsync(IInputType inputType);
}
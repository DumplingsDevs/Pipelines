namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherStringResult
{
    public Task<string> SendAsync(IInputType inputType);
}
namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherTaskStringResult
{
    public Task<string> SendAsync(IInputType inputType);
}
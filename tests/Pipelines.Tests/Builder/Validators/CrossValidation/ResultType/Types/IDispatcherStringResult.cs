namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherStringResult
{
    public string SendAsync(IInputType inputType);
}
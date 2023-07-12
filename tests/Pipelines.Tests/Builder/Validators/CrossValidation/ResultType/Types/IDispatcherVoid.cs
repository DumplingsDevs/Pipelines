namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherVoid
{
    public void SendAsync(IInputType inputType);
}
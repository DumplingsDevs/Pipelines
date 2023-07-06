namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherTaskGenericResult
{
    public Task<TResult> SendAsync<TResult>(IInputType inputType);
}
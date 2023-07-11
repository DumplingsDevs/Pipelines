namespace Pipelines.Tests.Builder.Validators.CrossValidation.ResultType.Types;

public interface IDispatcherTaskGenericResult
{
    public Task<TDispatcherResult> SendAsync<TDispatcherResult>(IInputType inputType);
}
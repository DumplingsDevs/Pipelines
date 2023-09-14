namespace Pipelines.Tests.UseCases.HandlersAssemblyNotProvided.Types;

public interface IDispatcher
{
    public Task<TResult> SendAsync<TResult>(IInput<TResult> input, CancellationToken token);
}
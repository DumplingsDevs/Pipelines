namespace Pipelines.Tests.UseCases.SyncNotGenericResult.Types;

public interface IDispatcher
{
    public string Send(IInput inputWithResult);
}
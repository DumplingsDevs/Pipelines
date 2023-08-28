namespace Pipelines.Tests.UseCases.SyncNotGenericResult.Types;

public interface IHandler<in TInput> where TInput : IInput
{
    public string Handle(TInput command);
}
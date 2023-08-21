namespace Pipelines.Tests.UseCases.SyncNotGenericResult.Types;

public interface IHandler<in TCommand> where TCommand : IInput
{
    public string Handle(TCommand command);
}
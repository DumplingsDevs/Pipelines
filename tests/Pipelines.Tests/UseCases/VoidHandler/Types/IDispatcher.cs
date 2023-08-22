namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface IDispatcher
{
    public void Send(IInput input);
}
namespace Pipelines.Tests.UseCases.VoidHandler.Types;

public interface IHandler<in TInput> where TInput : IInput
{
    public void Handle(TInput command);
}
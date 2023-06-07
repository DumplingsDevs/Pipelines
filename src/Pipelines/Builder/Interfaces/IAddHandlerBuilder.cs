using System.Reflection;

namespace Pipelines.Builder.Interfaces;

public interface IAddHandlerBuilder
{
    public IAddDispatcherBuilder AddHandler(Type type, Assembly assembly);
}
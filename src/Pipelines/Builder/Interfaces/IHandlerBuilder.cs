using System.Reflection;

namespace Pipelines.Builder.Interfaces;

public interface IHandlerBuilder
{
    public IDispatcherBuilder AddHandler(Type handlerType, Assembly assembly);
}
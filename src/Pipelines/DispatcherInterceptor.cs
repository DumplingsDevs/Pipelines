using System.Reflection;
using Pipelines.Exceptions;

namespace Pipelines;

internal class DispatcherInterceptor : DispatchProxy
{
    private Type _handlerInterfaceType = null!;

    private IServiceProvider _serviceProvider = null!;

    public static T Create<T>(IServiceProvider serviceProvider, Type handlerInterfaceType)
    {
        object proxy = Create<T, DispatcherInterceptor>()!;
        ((DispatcherInterceptor)proxy)._serviceProvider = serviceProvider;
        ((DispatcherInterceptor)proxy)._handlerInterfaceType = handlerInterfaceType;

        return (T)proxy;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ValidateArgs(args);

        return HandleExecutedMethod(args!);
    }

    private object? HandleExecutedMethod(object[] args)
    {
        ValidateArgs(args);

        var inputType = GetInputType(args);

        var handler = GetHandlerService(inputType);

        var method = GetHandlerMethod(handler, _handlerInterfaceType);

        return method.Invoke(handler, args);
    }

    private static MethodInfo GetHandlerMethod(object handler, Type handlerInterfaceType)
    {
        var handlerType = handler.GetType();

        // get the interface implemented by handler which matches handlerInterfaceType
        var implementedInterface = handlerType.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);
        
        // get the map of methods implemented by the interface (there will be always one method, because validators will check it)
        var interfaceMap = handlerType.GetInterfaceMap(implementedInterface);
        return interfaceMap.InterfaceMethods.First();
    }

    private object GetHandlerService(Type inputType)
    {
        var resultType = GetResultType(inputType);

        var handlerTypeWithInput = resultType is null
            ? _handlerInterfaceType.MakeGenericType(inputType)
            : _handlerInterfaceType.MakeGenericType(inputType, resultType);

        var handler = _serviceProvider.GetService(handlerTypeWithInput);

        if (handler is null)
        {
            throw new HandlerNotRegisteredException();
        }

        return handler;
    }

    private void ValidateArgs(object?[]? args)
    {
        var input = args?.FirstOrDefault();

        if (input is null)
        {
            throw new InputNullReferenceException();
        }

        // TO DO - validate args and their type.
        // Remember that we can pass CancellationToken.
        // Also in args there will be object that implements e.g. ICommand so we need to check if that object implements required input type
    }

    private static Type GetInputType(object[] args)
    {
        return args.First().GetType();
    }

    private static Type? GetResultType(Type queryType)
    {
        //TO DO- get only proper Interface
        //        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IQuery<>));

        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault();
        return queryInterfaceType?.GetGenericArguments().FirstOrDefault();
    }
}
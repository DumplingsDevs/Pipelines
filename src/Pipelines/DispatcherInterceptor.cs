using System.Reflection;
using Pipelines.Exceptions;

namespace Pipelines;

internal class DispatcherInterceptor : DispatchProxy
{
    private Type _handlerInterfaceType = null!;
    private Type _inputInterfaceType = null!;

    private IServiceProvider _serviceProvider = null!;

    public static T Create<T>(IServiceProvider serviceProvider, Type handlerInterfaceType, Type inputInterfaceType)
    {
        object proxy = Create<T, DispatcherInterceptor>()!;
        ((DispatcherInterceptor)proxy)._serviceProvider = serviceProvider;
        ((DispatcherInterceptor)proxy)._handlerInterfaceType = handlerInterfaceType;
        ((DispatcherInterceptor)proxy)._inputInterfaceType = inputInterfaceType;

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

        var handler = GetHandlerService(inputType, _inputInterfaceType);

        var method = GetHandlerMethod(handler, _handlerInterfaceType);

        return method.Invoke(handler, args);
    }

    private static MethodInfo GetHandlerMethod(object handler, Type handlerInterfaceType)
    {
        var handlerType = handler.GetType();

        // get the interface implemented by handler which matches handlerInterfaceType
        var implementedInterface = handlerType.GetInterfaces()
            .First(i => i.GetGenericTypeDefinition() == handlerInterfaceType);

        // get the map of methods implemented by the interface (there will be always one method, because validators will check it)
        var interfaceMap = handlerType.GetInterfaceMap(implementedInterface);
        return interfaceMap.InterfaceMethods.First();
    }

    private object GetHandlerService(Type inputType, Type inputInterfaceType)
    {
        var typesForGenericType = new List<Type> { inputType };
       
        var resultType = GetResultType(inputType, inputInterfaceType);
        if (resultType is not null)
        {
            typesForGenericType.AddRange(resultType);
        }

        var handlerTypeWithInput = _handlerInterfaceType.MakeGenericType(typesForGenericType.ToArray());
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
    }

    private static Type GetInputType(object[] args)
    {
        return args.First().GetType();
    }

    private static Type[]? GetResultType(Type queryType, Type inputTypeInterface)
    {
        var queryInterfaceType =
            queryType.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == inputTypeInterface);
        
        return queryInterfaceType?.GetGenericArguments();
    }
}
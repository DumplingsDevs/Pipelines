using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Exceptions;

namespace Pipelines;

internal class DispatcherInterceptor : DispatchProxy
{
    private Type _handlerInterfaceType = null!;
    private Type _inputInterfaceType = null!;
    private IServiceProvider _serviceProvider = null!;
    private DispatcherOptions _dispatcherOptions = null!;

    public static T Create<T>(IServiceProvider serviceProvider, Type handlerInterfaceType, Type inputInterfaceType,
        DispatcherOptions dispatcherOptions)
    {
        object proxy = Create<T, DispatcherInterceptor>()!;
        ((DispatcherInterceptor)proxy)._serviceProvider = serviceProvider;
        ((DispatcherInterceptor)proxy)._handlerInterfaceType = handlerInterfaceType;
        ((DispatcherInterceptor)proxy)._inputInterfaceType = inputInterfaceType;
        ((DispatcherInterceptor)proxy)._dispatcherOptions = dispatcherOptions;

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

        if (!_dispatcherOptions.CreateDIScope) 
            return InvokeHandlers(args, inputType, _serviceProvider);
        
        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        return InvokeHandlers(args, inputType, serviceProvider);
    }

    private object? InvokeHandlers(object[] args, Type inputType, IServiceProvider serviceProvider)
    {
        var handlers = GetHandlerService(inputType, _inputInterfaceType, serviceProvider);

        if (handlers.Count == 1)
        {
            var handler = handlers.First();
            var method = GetHandlerMethod(handlers.First(), _handlerInterfaceType);
            return method.Invoke(handler, args);
        }

        object? result = null;
        foreach (var handler in handlers)
        {
            var method = GetHandlerMethod(handlers.First(), _handlerInterfaceType);
            result = method.Invoke(handler, args);
        }

        return result;
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

    private List<object?> GetHandlerService(Type inputType, Type inputInterfaceType, IServiceProvider serviceProvider)
    {
        var typesForGenericType = new List<Type> { inputType };

        var resultType = GetResultType(inputType, inputInterfaceType);
        if (resultType is not null)
        {
            typesForGenericType.AddRange(resultType);
        }

        var handlerTypeWithInput = _handlerInterfaceType.MakeGenericType(typesForGenericType.ToArray());
        var handlers = serviceProvider.GetServices(handlerTypeWithInput);

        var handlerService = handlers.ToList();

        if (handlers is null || !handlerService.Any())
        {
            throw new HandlerNotRegisteredException(handlerTypeWithInput);
        }

        return handlerService;
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
            queryType.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == inputTypeInterface);

        return queryInterfaceType?.GetGenericArguments();
    }
}
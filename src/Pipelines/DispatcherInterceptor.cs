using System.Reflection;
using Castle.DynamicProxy;
using Pipelines.Exceptions;

namespace Pipelines;

public class DispatcherInterceptor : IInterceptor
{
    private readonly Type _handlerType;
    private readonly Type _inputType;
    private readonly IServiceProvider _serviceProvider;

    public DispatcherInterceptor(IServiceProvider serviceProvider, Type inputType, Type handlerType)
    {
        _serviceProvider = serviceProvider;
        _inputType = inputType;
        _handlerType = handlerType;
    }

    public void Intercept(IInvocation invocation)
    {
        var arguments = invocation.Arguments;

        var result = HandleExecutedMethod(arguments);
        if (result is not null)
        {
            invocation.ReturnValue = result;
        }
    }

    private object? HandleExecutedMethod(object[] args)
    {
        ValidateArgs(args);

        var inputType = args.First().GetType();

        var handler = GetHandlerService(inputType);

        var method = GetHandlerMethod(handler, inputType);

        return method.Invoke(handler, args);
    }

    private static MethodInfo GetHandlerMethod(object handler, Type inputType)
    {
        var methods = handler.GetType()
            .GetMethods()
            .Where(x => x.GetParameters()
                .Any(y => y.ParameterType == inputType))
            .ToList();

        switch (methods.Count)
        {
            case 0:
                throw new HandlerMethodNotImplementedException(handler.GetType().Name, inputType.Name);
            case > 1:
                throw new MultipleHandlerMethodsException(inputType.Name);
        }

        var method = methods.First();

        // TO DO think how to validate method - do we need to validate method? Maybe it's enough to validate HandlerType interface in builder

        return method;
    }

    private object GetHandlerService(Type inputType)
    {
        var resultType = GetResultType(inputType);

        var handlerTypeWithInput = resultType is null
            ? _handlerType.MakeGenericType(inputType)
            : _handlerType.MakeGenericType(inputType, resultType);

        var handler = _serviceProvider.GetService(handlerTypeWithInput);

        if (handler is null)
        {
            throw new HandlerNotRegisteredException();
        }

        return handler;
    }

    private void ValidateArgs(object[] args)
    {
        var input = args.FirstOrDefault();

        if (input is null)
        {
            throw new InputNullReferenceException();
        }

        // TO DO - validate args and their type.
        // Remember that we can pass CancellationToken.
        // Also in args there will be object that implements e.g. ICommand so we need to check if that object implements required input type
    }

    private static Type? GetResultType(Type queryType)
    {
        //TO DO- get only proper Interface
        //        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IQuery<>));

        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault();
        return queryInterfaceType?.GetGenericArguments().FirstOrDefault();
    }
}
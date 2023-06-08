using System.Reflection;
using Castle.DynamicProxy;

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

        // invocation.ReturnValue = "Tutaj trzeba ustawi zwracaną wartośc - czyli w naszym przypadku będzie to rezultat z handlera";
        // invocation.Proceed(); - tego nie używamy ponieważ nie ma klasy (to działa trochę jak pipeline - może będziemy mogli to wykorzystac do implementacji behavior?
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
                throw new Exception($"(TODO) Cannot find method in {handler.GetType().Name} which accepts input of type {inputType.Name}");
            case > 1:
                throw new Exception($"(TODO) handler should implement only one method with input of type {inputType}");
        }

        var method = methods.First();

        // think how to validate method because basically we have two types of method:
        // - only with input
        // - with input and CancellationToken

        return method;
    }

    private object GetHandlerService(Type inputType)
    {
        var resultType = GetResultType(inputType);

        var handlerTypeWithInput = resultType is null
            ? _handlerType.MakeGenericType(inputType)
            : _handlerType.MakeGenericType(inputType, resultType);

        var handler =  _serviceProvider.GetService(handlerTypeWithInput);
        if (handler is null)
        {
            throw new Exception("(TODO) Handler is not registered");
        }

        return handler;
    }

    private void ValidateArgs(object[] args)
    {
        // Exception when args == 0 || args > 1
        // var input = args.FirstOrDefault();
        // Exception when input is null
        // Exception when input.GetType != _inputType 
    }

    private static Type? GetResultType(Type queryType)
    {
        //TO DO- get only proper Interface
        //        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IQuery<>));

        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault();
        return queryInterfaceType?.GetGenericArguments().FirstOrDefault();
    }
}
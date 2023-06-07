using Castle.DynamicProxy;

namespace Pipelines;

public class DispatcherInterceptor : IInterceptor
{
    private readonly Type _handlerType;
    private readonly IServiceProvider _serviceProvider;

    public DispatcherInterceptor(IServiceProvider serviceProvider, Type handlerType)
    {
        _handlerType = handlerType;
        _serviceProvider = serviceProvider;
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
        // for now get first - but we should filter args and look for object that implements input interface - or just try to match the best method
        var input = args.FirstOrDefault();
        var inputType = input.GetType();
        var resultType = GetResultType(inputType);

        Type handlerTypeWithInput;
        if (resultType is null)
        {
            handlerTypeWithInput = _handlerType.MakeGenericType(inputType);
        }
        else
        {
            var inputAndResultTypes = new[] { inputType, resultType };
            handlerTypeWithInput = _handlerType.MakeGenericType(inputAndResultTypes);
        }

        var handler = _serviceProvider.GetService(handlerTypeWithInput);

        // What if handler contains more items 
        var method = handler.GetType().GetMethods()
            .FirstOrDefault(x => x.GetParameters().Any(y => y.ParameterType == inputType));

        return method.Invoke(handler, args);


        // find handler
        // trigger method
        //return result
    }

    private static Type? GetResultType(Type queryType)
    {
        //TO DO- get only proper Interface
        //        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault(x => x.GetGenericTypeDefinition() == typeof(IQuery<>));

        var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault();
        return queryInterfaceType?.GetGenericArguments().FirstOrDefault();
    }
}
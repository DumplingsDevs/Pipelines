namespace Pipelines.Builder.Validators.Decorator.ImplementsInterface;

internal static class ImplementsInterfaceValidator
{
    internal static void Validate(Type decoratorType, Type handlerInterfaceType)
    {
        var isImplementsInterface = false;
        
        foreach (var interfaceType in decoratorType.GetInterfaces())
        {
            if (interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == handlerInterfaceType)
            {
                isImplementsInterface = true;
                break;
            }
        }

        if (!isImplementsInterface)
        {
            throw new Exception();
        }
    }
}
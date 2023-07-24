using Pipelines.Builder.Validators.Decorator.Constructor;
using Pipelines.Builder.Validators.Decorator.ImplementsInterface;

namespace Pipelines.Builder.Validators.Decorator;

internal static class DecoratorValidator
{
    internal static void Validate(Type decoratorType, Type handlerType)
    {
        DecoratorConstructorValidator.Validate(decoratorType, handlerType);
        ImplementsInterfaceValidator.Validate(decoratorType, handlerType);
    }
}
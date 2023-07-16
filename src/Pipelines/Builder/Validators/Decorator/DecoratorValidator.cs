using Pipelines.Builder.Validators.Decorator.Constructor;

namespace Pipelines.Builder.Validators.Decorator;

internal static class DecoratorValidator
{
    internal static void Validate(Type decoratorType, Type expectedParameterType)
    {
        // Validate if contains proper generic implementation
        DecoratorConstructorValidator.Validate(decoratorType, expectedParameterType);
    }
}
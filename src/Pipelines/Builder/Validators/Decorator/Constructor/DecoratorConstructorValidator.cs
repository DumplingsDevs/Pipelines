using Pipelines.Builder.Validators.Decorator.Constructor.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Decorator.Constructor;

internal static class DecoratorConstructorValidator
{
    internal static void Validate(Type decoratorType, Type expectedParameterType)
    {
        var hasExpectedParameter = decoratorType.ConstructorHasCompatibleGenericType(expectedParameterType);
        if (!hasExpectedParameter)
        {
            throw new ConstructorValidationException(decoratorType, expectedParameterType);
        }
    }
}
namespace Pipelines.Builder.Validators.Decorator.Constructor.Exceptions;

public class ConstructorValidationException : Exception
{
    private const string ErrorMessage =
        "Decorator type {0} does not have a compatible generic type with {1}.";
    public ConstructorValidationException(Type decoratorType, Type expectedParameterType)
        : base(string.Format(ErrorMessage, decoratorType.Name, expectedParameterType.Name))
    { }
}
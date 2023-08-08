namespace Pipelines.Builder.Validators.Shared.MethodWithOneParameter.Exceptions;

public class MethodShouldHaveAtLeastOneParameterException : Exception
{
    internal MethodShouldHaveAtLeastOneParameterException(string methodName, Type type)
        : base($"Method '{methodName}' of type '{type.Name}' should have at least one parameter.")
    {
    }
}
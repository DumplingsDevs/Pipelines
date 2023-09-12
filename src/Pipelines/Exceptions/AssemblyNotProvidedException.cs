namespace Pipelines.Exceptions;

public class AssemblyNotProvidedException : Exception
{
    private const string ErrorMessage = "The method {0} requires at least one provided assembly.";

    public AssemblyNotProvidedException(string methodName) : base(string.Format(ErrorMessage,
        methodName))
    { }
}
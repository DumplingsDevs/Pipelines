namespace Pipelines.Builder.Validators.Shared.MethodResultTypes.Exceptions;

public class ExpectedVoidMethodException : Exception
{
    private const string ErrorMessage = "Expected void method, but found method with result in type: ";

    public ExpectedVoidMethodException(Type type) : base(ErrorMessage + type.Name) { }
}
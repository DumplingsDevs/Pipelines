namespace Pipelines.Exceptions;

public class HandleMethodNotFoundException : Exception
{
    private const string ErrorMessage = "Handle method not found in type: ";

    public HandleMethodNotFoundException(Type type) : base(ErrorMessage + type.Name) { }
}
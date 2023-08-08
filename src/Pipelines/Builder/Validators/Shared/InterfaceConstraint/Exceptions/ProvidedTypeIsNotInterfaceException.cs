namespace Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions;

public class ProvidedTypeIsNotInterfaceException : Exception
{
    private const string ErrorMessage = "The provide type '{0}' is not interface.";

    internal ProvidedTypeIsNotInterfaceException(Type type) : base(string.Format(ErrorMessage, type.Name))
    {
    }
}
namespace Pipelines.Builder.Validators.Shared.InterfaceConstraint.Exceptions;

internal class ProvidedTypeIsNotInterfaceException : Exception
{
    private const string ErrorMessage = "The provide type '{0}' is not interface.";

    internal ProvidedTypeIsNotInterfaceException(Type type) : base(string.Format(ErrorMessage, type.Name))
    {
    }
}
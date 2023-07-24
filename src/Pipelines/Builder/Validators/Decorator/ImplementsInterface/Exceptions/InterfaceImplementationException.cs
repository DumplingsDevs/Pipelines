namespace Pipelines.Builder.Validators.Decorator.ImplementsInterface.Exceptions;

internal class InterfaceImplementationException : Exception
{
    private const string ErrorMessage =
        "Decorator type {0} does not implement interface {1}.";
    
    internal InterfaceImplementationException(Type decoratorType, Type handlerInterfaceType)
        : base(string.Format(ErrorMessage, decoratorType.Name, handlerInterfaceType.Name))
    { }
}
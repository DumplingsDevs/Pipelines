namespace Pipelines.Builder.Exceptions;

internal class DispatcherNotRegisteredException : Exception
{
    private static readonly string ErrorMessage =
        "There is not dispatcher registration in DI of {0} type. Make sure that Pipelines.Generator is added to your project. If you do not want to use Pipelines.Generators you can use Proxy dispatcher - set UseReflectionProxyImplementation to true when calling AddDispatcher.";

    internal DispatcherNotRegisteredException(Type dispatcherType) : base(string.Format(ErrorMessage, dispatcherType.Name))
    {
    }
}
namespace Pipelines.Builder.Interfaces;

public class DispatcherNotRegisteredException : Exception
{
    private static readonly string ErrorMessage =
        "There is not dispatcher registration in DI of {0} type. Probably something went wrong with source generator. You can make workaround and use Proxy dispatcher - set UseReflectionProxyImplementation to true when calling AddDispatcher.";

    public DispatcherNotRegisteredException(Type dispatcherType) : base(string.Format(ErrorMessage, dispatcherType.Name))
    {
    }
}
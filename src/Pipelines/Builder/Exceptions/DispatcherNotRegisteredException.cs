namespace Pipelines.Builder.Exceptions;

public class DispatcherNotRegisteredException : Exception
{
    private static readonly string ErrorMessage =
        "There is not dispatcher registration in DI of {0} type. Make sure that Pipelines.Generator is added to your project. Make sure that typeof operator is usd in pipelines builder directly e.g. AddInput(typeof(IInputType)). If you do not want to use Pipelines.WrapperDispatcherGenerator you can use Proxy dispatcher - set UseReflectionProxyImplementation to true when calling AddDispatcher.";

    internal DispatcherNotRegisteredException(Type dispatcherType) : base(string.Format(ErrorMessage, dispatcherType.Name))
    {
    }
}
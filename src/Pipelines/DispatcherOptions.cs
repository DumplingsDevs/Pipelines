namespace Pipelines;

public class DispatcherOptions
{
    public DispatcherOptions(bool useReflectionProxyImplementation)
    {
        UseReflectionProxyImplementation = useReflectionProxyImplementation;
    }

    public DispatcherOptions()
    {
        UseReflectionProxyImplementation = false;
    }
    
    public bool UseReflectionProxyImplementation { get; set; }
    public bool CreateDIScope { get; set; }
    public bool ThrowExceptionIfHandlerNotFound { get; set; }
}
namespace Pipelines;

public class DispatcherOptions
{
    public DispatcherOptions()
    {
        UseReflectionProxyImplementation = false;
        CreateDIScope = true;
        ThrowExceptionIfHandlerNotFound = true;
    }
    
    public bool UseReflectionProxyImplementation { get; set; }
    public bool CreateDIScope { get; set; }
    public bool ThrowExceptionIfHandlerNotFound { get; set; }
}
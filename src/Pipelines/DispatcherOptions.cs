namespace Pipelines;

public class DispatcherOptions
{
    public bool UseReflectionProxyImplementation { get; set; }
    public bool CreateDIScope { get; set; }
    public bool ThrowExceptionIfHandlerNotFound { get; set; }
}
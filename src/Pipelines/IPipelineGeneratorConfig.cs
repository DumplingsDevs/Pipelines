namespace Pipelines;

public interface IPipelineGeneratorConfig
{
    /// <summary>
    /// Return type of dispatcher using typeof()
    /// Type GetDispatcher => typeof(IRequestDispatcher)
    /// </summary>
    Type GetDispatcherType { get; }
    Type GetInputType { get; }
    Type GetHandlerType { get; }
}
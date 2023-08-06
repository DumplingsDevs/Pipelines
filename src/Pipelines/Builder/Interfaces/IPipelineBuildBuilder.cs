namespace Pipelines.Builder.Interfaces;

public interface IPipelineBuildBuilder
{
    /// <summary>
    /// Builds the pipeline with the specified configurations. Register all types in Dependency Injection Container.
    /// </summary>
    public void Build();
}
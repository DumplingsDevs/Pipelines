using Microsoft.CodeAnalysis;

namespace Pipelines.Generators.Models;

internal class PipelineConfig
{
    public PipelineConfig(INamedTypeSymbol dispatcherType, INamedTypeSymbol handlerType, INamedTypeSymbol inputType)
    {
        DispatcherType = dispatcherType;
        HandlerType = handlerType;
        InputType = inputType;
    }

    public INamedTypeSymbol DispatcherType { get; }
    public INamedTypeSymbol HandlerType { get; }
    public INamedTypeSymbol InputType { get; }
}

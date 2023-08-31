using Microsoft.CodeAnalysis;

namespace Pipelines.NativeDispatcherGenerator.Models;

internal record PipelineConfig(INamedTypeSymbol DispatcherType, INamedTypeSymbol HandlerType, INamedTypeSymbol InputType)
{
    public INamedTypeSymbol DispatcherType { get; } = DispatcherType;
    public INamedTypeSymbol HandlerType { get; } = HandlerType;
    public INamedTypeSymbol InputType { get; } = InputType;
}

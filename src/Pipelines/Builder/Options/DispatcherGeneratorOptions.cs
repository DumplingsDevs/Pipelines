namespace Pipelines.Builder.Options;

public class DispatcherGeneratorOptions
{
    public DispatcherGeneratorOptions(Type dispatcherType, DispatcherOptions options)
    {
        DispatcherType = dispatcherType;
        Options = options;
    }

    public Type DispatcherType { get; }
    public DispatcherOptions Options { get; }
}
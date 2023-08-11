namespace Pipelines;

public class DecoratorOptions
{
    public DecoratorOptions(bool strictMode)
    {
        StrictMode = strictMode;
    }
    
    public DecoratorOptions()
    {
        StrictMode = true;
    }

    public bool StrictMode { get; }
};
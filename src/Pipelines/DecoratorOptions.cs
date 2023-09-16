namespace Pipelines;

public class DecoratorOptions
{
    /// <summary>
    /// <para>Default value: `true`</para>
    /// 
    /// <para>Choose whether Validators should throw an exception when the Pipeline Builder detects an incorrectly implemented decorator</para>
    /// 
    /// <para>When true:  If the Pipeline Builder detects an incorrect Decorator implementation, it will throw an exception. </para>
    /// <para>When false:  If the Pipeline Builder detects an incorrect Decorator implementation, it will skip this decorator. The decorator will not be applied to handlers, and no exception will be thrown. </para>
    /// </summary>
    public bool StrictMode { get; } = true;
};
namespace Pipelines;

public class DispatcherOptions
{
    /// <summary>
    /// <para>Default value: `false`</para>
    /// 
    /// <para>Choose the type of Dispatcher your application would like to use</para>
    /// 
    /// <para>When true: The Dispatcher uses an implementation based on reflection. </para>
    /// <para>When false: The Dispatcher uses a source-generated dispatcher. </para>
    /// </summary>
    public bool UseReflectionProxyImplementation { get; set; } = false;

    /// <summary>
    /// <para>Default value: `true`</para>
    /// 
    /// <para>Determines whether to create a new dependency injection scope when dispatching inputs.</para>
    /// 
    /// <para>When true: The Dispatcher creates a new Dependency Injection scope during processing. </para>
    /// <para>When false: The Dispatcher uses the existing scope. </para>
    /// </summary>
    public bool CreateDIScope { get; set; } = true;

    /// <summary>
    /// <para>Default value: `true`</para>
    /// 
    /// <para>Determines whether to throw a `HandlerNotRegisteredException` if no handler is found for the given input.</para>
    /// <para>**NOTE**: This configuration only applies to Pipelines that don't return results (i.e., `void` or `Task`)..</para>
    /// 
    /// <para>When true: If no handler is found, the Dispatcher will throw a `HandlerNotRegisteredException`. </para>
    /// <para>When false: The Dispatcher will proceed without throwing an exception if no handler is found. </para>
    /// </summary>
    public bool ThrowExceptionIfHandlerNotFound { get; set; } = true;
}
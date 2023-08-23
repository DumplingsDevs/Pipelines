namespace Pipelines.Tests;

public static class EnvVariables
{
    public static bool UseReflectionProxyImplementation { get; }
    
    static EnvVariables()
    {
        DotNetEnv.Env.NoClobber().Load();
        var envUseReflectionProxyImplementation = Environment.GetEnvironmentVariable("USE_REFLECTION_PROXY_IMPLEMENTATION");
        var envParsedSuccessfully = bool.TryParse(envUseReflectionProxyImplementation, out var useReflectionProxyImplementation);

        UseReflectionProxyImplementation = true;
    }
}
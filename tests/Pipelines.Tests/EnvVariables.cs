namespace Pipelines.Tests;

public static class EnvVariables
{
    public static bool UseReflectionProxyImplementation { get; }
    
    static EnvVariables()
    {
        DotNetEnv.Env.NoClobber().Load();
        var envUseReflectionProxyImplementation = Environment.GetEnvironmentVariable("UseReflectionProxyImplementation");
        var envParsedSuccessfully = bool.TryParse(envUseReflectionProxyImplementation, out var useReflectionProxyImplementation);

        UseReflectionProxyImplementation = envParsedSuccessfully && useReflectionProxyImplementation;
    }
}
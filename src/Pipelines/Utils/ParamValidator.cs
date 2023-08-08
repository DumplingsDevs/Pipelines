namespace Pipelines.Utils;

internal static class ParamValidator
{
    internal static void NotNull(object parameter, string parameterName)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
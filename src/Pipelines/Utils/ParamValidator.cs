namespace Pipelines.Utils;

public static class ParamValidator
{
    public static void NotNull(object parameter, string parameterName)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
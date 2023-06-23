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

    public static void NotNullOrEmpty<T>(T[] array, string arrayName)
    {
        if (array == null)
        {
            throw new ArgumentNullException(arrayName);
        }
        
        if (array.Length < 1)
        {
            throw new ArgumentException("Array must have at least one element", arrayName);
        }
    }
}
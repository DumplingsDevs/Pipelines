namespace Pipelines;

internal class InputNullReferenceException : Exception
{
    public InputNullReferenceException() : base("Cannot handle input with null value")
    {
        
    }
}
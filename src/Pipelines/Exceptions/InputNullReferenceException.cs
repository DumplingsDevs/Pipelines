namespace Pipelines.Exceptions;

public class InputNullReferenceException : Exception
{
    private const string ErrorMessage = "Cannot handle input with null value";
    public InputNullReferenceException() : base(ErrorMessage)
    {
        
    }
}
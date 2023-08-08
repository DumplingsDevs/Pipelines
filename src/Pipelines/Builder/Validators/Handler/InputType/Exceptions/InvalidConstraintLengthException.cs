namespace Pipelines.Builder.Validators.Handler.InputType.Exceptions;

public class InvalidConstraintLengthException : Exception
{
    private const string ErrorMessageFormat = "Invalid number of generic parameter constraints in type {0}";

    internal InvalidConstraintLengthException(Type type) : base(string.Format(ErrorMessageFormat, type.Name)) { }
}
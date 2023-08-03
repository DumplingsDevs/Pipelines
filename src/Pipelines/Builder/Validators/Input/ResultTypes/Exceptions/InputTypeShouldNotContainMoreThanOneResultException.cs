namespace Pipelines.Builder.Validators.Input.ResultTypes.Exceptions;

internal class InputTypeShouldNotContainMoreThanOneResultException : Exception
{
    private const string ErrorMessage = "Input should contains zero or one Result Type but contains {0}. Input Type {1}";

    internal InputTypeShouldNotContainMoreThanOneResultException(int length, Type inputType) : base(string.Format(ErrorMessage, length, inputType))
    { }
}
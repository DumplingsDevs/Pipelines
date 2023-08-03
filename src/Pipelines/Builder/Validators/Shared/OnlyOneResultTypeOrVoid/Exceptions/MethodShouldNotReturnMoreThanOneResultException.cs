namespace Pipelines.Builder.Validators.Shared.OnlyOneResultTypeOrVoid.Exceptions;

internal class MethodShouldNotReturnMoreThanOneResultException : Exception
{
    private const string ErrorMessage = "Type should contains zero or one Result Type but contains {0}. Type {1}";

    internal MethodShouldNotReturnMoreThanOneResultException(int count, Type type): base(string.Format(ErrorMessage, count, type))
    { }
}
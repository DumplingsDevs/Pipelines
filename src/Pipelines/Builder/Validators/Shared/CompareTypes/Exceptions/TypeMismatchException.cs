namespace Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;

internal class TypeMismatchException : Exception
{
    private const string ErrorMessage =
        "Type mismatch in provided type '{0}' from '{1}' and type '{2}' from '{3}'";

    internal TypeMismatchException(Type type1, Type type2, Type sourceType1, Type sourceType2) : base(
        string.Format(ErrorMessage, type1, sourceType1, type2, sourceType2))
    {
    }
}
namespace Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;

internal class GenericTypeMismatchException : Exception
{
    private const string ErrorMessage =
        "Generic type mismatch in provided type '{0}' from '{1}' and type '{2}' from '{3}'";

    internal GenericTypeMismatchException(Type type1, Type type2, Type sourceType1, Type sourceType2)
        : base(string.Format(ErrorMessage, type1, sourceType1, type2, sourceType2))
    {
    }
}
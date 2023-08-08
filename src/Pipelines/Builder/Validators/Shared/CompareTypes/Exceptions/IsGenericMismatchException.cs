namespace Pipelines.Builder.Validators.Shared.CompareTypes.Exceptions;

public class IsGenericMismatchException : Exception
{
    private const string ErrorMessage =
        "One of provided parameters is generic and another is no generic. Type '{0}' Generic:'{1}' SourceType:'{2}', Type '{3}' Generic: '{4}' SourceType '{5}'";

    internal IsGenericMismatchException(Type type1, bool isType1Generic, Type sourceType1, Type isGenericType2,
        bool isType2Generic, Type sourceType2) : base(
        string.Format(ErrorMessage, type1, isType1Generic, sourceType1, isGenericType2, isType2Generic, sourceType2))
    { }
}
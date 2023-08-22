using Microsoft.CodeAnalysis;
using Pipelines.Generators.Exceptions;

namespace Pipelines.Generators.Validators.CrossValidation.Exceptions;

internal class ReturnTypeMismatchException : GeneratorException
{
    private const string ErrorMessage =
        "Method {0} from type {1} and method {2} from type {3} do not have the same return type.";

    public ReturnTypeMismatchException(ISymbol firstType, ISymbol firstMethod,
        ISymbol secondType, ISymbol secondMethod) : base(string.Format(ErrorMessage, firstMethod.Name, firstType.Name,
        secondMethod.Name, secondType.Name))

    {
    }
}
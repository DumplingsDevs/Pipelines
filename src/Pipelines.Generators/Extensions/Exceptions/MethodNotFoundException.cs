using Microsoft.CodeAnalysis;
using Pipelines.Generators.Exceptions;

namespace Pipelines.Generators.Extensions.Exceptions;

internal class MethodNotFoundException : GeneratorException
{
    private const string ErrorMessage = "Type {0} does not have any method.";
    public MethodNotFoundException(INamedTypeSymbol typeSymbol) : base(string.Format(ErrorMessage, typeSymbol.Name))
    {
    }
}

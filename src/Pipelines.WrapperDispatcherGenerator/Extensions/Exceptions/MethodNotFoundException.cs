using Microsoft.CodeAnalysis;
using Pipelines.WrapperDispatcherGenerator.Exceptions;

namespace Pipelines.WrapperDispatcherGenerator.Extensions.Exceptions;

internal class MethodNotFoundException : GeneratorException
{
    private const string ErrorMessage = "Type {0} does not have any method.";
    public MethodNotFoundException(INamedTypeSymbol typeSymbol) : base(string.Format(ErrorMessage, typeSymbol.Name))
    {
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Pipelines.NativeDispatcherGenerator.Exceptions;

namespace Pipelines.NativeDispatcherGenerator.Validators.Dispatcher.Exceptions;

internal class ConstraintOnTypeParameterNotFoundException : GeneratorException
{
    private const string ErrorMessage = "Type parameters {0} should have defined constraint e.g. TResult : class";

    public ConstraintOnTypeParameterNotFoundException(List<ITypeParameterSymbol> parameterSymbols) : base(string.Format(
        ErrorMessage,
        string.Join(", ", parameterSymbols.Select(x => x.Name))))
    {
    }
}
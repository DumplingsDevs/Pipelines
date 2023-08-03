using System.Reflection;
using Pipelines.Builder.Validators.Shared.OnlyOneResultTypeOrVoid.Exceptions;
using Pipelines.Utils;

namespace Pipelines.Builder.Validators.Shared.OnlyOneResultTypeOrVoid;

internal static class OnlyOneResultTypeOrVoidValidator
{
    internal static void Validate(MethodInfo methodToValidate, Type type)
    {
        var methodReturnTypes = methodToValidate.GetReturnTypes();

        if (methodReturnTypes.Count > 1)
        {
            throw new MethodShouldNotReturnMoreThanOneResultException(methodReturnTypes.Count, type);
        }
    }
}
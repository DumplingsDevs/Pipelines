using Pipelines.Utils;

namespace Pipelines.Builder.Validators;

public class ValidateDispatcherMethod
{
    internal static void Validate(Type dispatcherType)
    {
        ParamValidator.NotNull(dispatcherType, nameof(dispatcherType));

        var handleMethod = dispatcherType.GetMethods().FirstOrDefault();
    }
}
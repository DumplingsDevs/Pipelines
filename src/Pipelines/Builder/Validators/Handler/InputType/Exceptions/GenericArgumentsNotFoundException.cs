namespace Pipelines.Builder.Validators.Handler.InputType.Exceptions;

public class GenericArgumentsNotFoundException : Exception
{
    private const string ErrorMessage = "No generic arguments found in type: ";

    internal GenericArgumentsNotFoundException(Type type) : base(ErrorMessage + type.Name) { }
}
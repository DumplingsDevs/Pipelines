namespace Pipelines.Exceptions;

public class GenericArgumentsNotFoundException : Exception
{
    private const string ErrorMessage = "No generic arguments found in type: ";

    public GenericArgumentsNotFoundException(Type type) : base(ErrorMessage + type.Name) { }
}
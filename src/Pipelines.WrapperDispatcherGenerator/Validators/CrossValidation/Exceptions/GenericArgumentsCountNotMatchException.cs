using Pipelines.WrapperDispatcherGenerator.Exceptions;

namespace Pipelines.WrapperDispatcherGenerator.Validators.CrossValidation.Exceptions;

internal class GenericArgumentsCountNotMatchException : GeneratorException
{
    private const string ErrorMessage =
        "Input, Handler and Dispatcher have different number of generic response parameters.";
    public GenericArgumentsCountNotMatchException() : base(ErrorMessage)
    {
    }
}

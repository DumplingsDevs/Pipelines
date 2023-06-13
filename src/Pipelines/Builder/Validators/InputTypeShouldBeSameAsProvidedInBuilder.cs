using Pipelines.Builder.Validators.Exceptions;

namespace Pipelines.Builder.Validators;

public static class InputTypeShouldBeSameAsProvidedInBuilder
{
    public static void Validate(Type inputType, params Type[] typesToValidate)
    {
        //TO DO add for null checks
        foreach (var type in typesToValidate)
        {
            var methodInfo = type.GetMethods().First();
            var methodInputType = methodInfo.GetParameters().First().ParameterType;
            if (inputType != methodInputType)
            {
                throw new InputTypeShouldBeSameAsProvidedInBuilderException(inputType, type);
            }
        }
    }
}
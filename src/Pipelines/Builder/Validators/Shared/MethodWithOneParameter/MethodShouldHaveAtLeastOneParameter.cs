namespace Pipelines.Builder.Validators.Shared.MethodWithOneParameter;

internal static class MethodShouldHaveAtLeastOneParameter
{
    internal static void Validate(Type typeToValidate)
    {
        var handleMethod = typeToValidate
            .GetMethods().First();

        var parametersCount = handleMethod.GetParameters().Length;
        if (parametersCount == 0)
        {
            throw new Exception();
        }
    }
}
namespace Pipelines.Builder.Validators;

internal static class MethodShouldHaveAtLeastOneParameter
{
    internal static void Validate(params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
            var handleMethod = type
                .GetMethods().First();

            var parametersCount = handleMethod.GetParameters().Length;
            if (parametersCount == 0)
            {
                throw new Exception();
            }
        }
    }
}
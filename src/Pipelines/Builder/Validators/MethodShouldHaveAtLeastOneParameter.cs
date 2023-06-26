namespace Pipelines.Builder.Validators;

internal class MethodShouldHaveAtLeastOneParameter
{
    internal static void Validate(params Type[] typesToValidate)
    {
        foreach (var type in typesToValidate)
        {
            var methods = type
                .GetMethods()
                .Where(x => x.IsPublic).ToList();
            
            //TO DO
        }
    }
}
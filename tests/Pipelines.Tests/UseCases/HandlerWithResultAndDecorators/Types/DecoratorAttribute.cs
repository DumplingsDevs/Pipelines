namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

public class DecoratorAttribute : Attribute
{
    public DecoratorAttribute(int index)
    {
        Index = index;
    }

    public int Index { get; }
}
using System.Reflection;
using Pipelines.Builder.Decorators;
using Pipelines.Tests.Builder.Decorators.Samples;
using Pipelines.Tests.Builder.Decorators.Types;


namespace Pipelines.Tests.Builder.Decorators;

public class WithAttributeTests
{
    private Assembly _assembly = typeof(WithAttributeTests).Assembly;
    private Type _handlerType = typeof(ICommandHandler<,>);

    [Test]
    public void HappyPath()
    {
        var types = DecoratorsBuilder.BuildDecorators(x => x.WithAttribute<DecoratorAttribute>(), _handlerType,
            _assembly);

        Assert.That(types, Has.Count.EqualTo(1));
        Assert.That(types.First().FullName, Is.EqualTo(typeof(FourthDecorator).FullName));
    }
}
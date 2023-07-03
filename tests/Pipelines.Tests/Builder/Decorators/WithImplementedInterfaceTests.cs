using System.Reflection;
using Pipelines.Builder.Decorators;
using Pipelines.Tests.Builder.Decorators.Samples;
using Pipelines.Tests.Builder.Decorators.Types;


namespace Pipelines.Tests.Builder.Decorators;

public class WithImplementedInterfaceTests
{
    private Assembly _assembly = typeof(WithImplementedInterfaceTests).Assembly;
    private Type _handlerType = typeof(ICommandHandler<,>);

    [Test]
    public void HappyPath()
    {
        var builder = new DecoratorsBuilder();

        builder.BuildDecorators(x => x.WithImplementedInterface<IDecoratorMarker>(), _handlerType, _assembly);

        var types = builder.GetDecorators();

        Assert.That(types, Has.Count.EqualTo(1));
        Assert.That(types.First().FullName, Is.EqualTo(typeof(FirstDecorator).FullName));
    }
}
using System.Reflection;
using Pipelines.Builder.Decorators;
using Pipelines.Tests.Builder.Decorators.Samples;
using Pipelines.Tests.Builder.Decorators.Types;


namespace Pipelines.Tests.Builder.Decorators;

public class WithNameContainingTests
{
    private Assembly _assembly = typeof(WithNameContainingTests).Assembly;
    private Type _handlerType = typeof(ICommandHandler<,>);

    [Test]
    public void HappyPath()
    {
        var builder = new DecoratorsBuilder();

        builder.BuildDecorators(x => x.WithNameContaining("02E6B297_F635_48D4_ABD9_D280AF6C3DB8"), _handlerType, _assembly);

        var types = builder.GetDecorators();

        Assert.That(types, Has.Count.EqualTo(1));
        Assert.That(types.First().FullName, Is.EqualTo(typeof(SecondDecorator02E6B297_F635_48D4_ABD9_D280AF6C3DB8).FullName));
    }
}
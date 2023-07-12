using System.Reflection;
using Pipelines.Builder.Decorators;
using Pipelines.Tests.Builder.Decorators.Samples;
using Pipelines.Tests.Builder.Decorators.Types;


namespace Pipelines.Tests.Builder.Decorators;

public class WithTests
{
    private Assembly _assembly = typeof(WithTests).Assembly;
    private Type _handlerType = typeof(ICommandHandler<,>);

    [Test]
    public void HappyPath()
    {
        var types = DecoratorsBuilder.BuildDecorators(
            x => x.With(y =>
                y.Namespace?.Contains("Pipelines.Tests.Builder.Decorators.Samples") ?? false),
            _handlerType, _assembly);
        
        Assert.That(types, Has.Count.EqualTo(4));
        Assert.That(types[0].FullName, Is.EqualTo(typeof(FirstDecorator).FullName));
        Assert.That(types[1].FullName, Is.EqualTo(typeof(FourthDecorator).FullName));
        Assert.That(types[2].FullName, Is.EqualTo(typeof(SecondDecorator02E6B297_F635_48D4_ABD9_D280AF6C3DB8).FullName));
        Assert.That(types[3].FullName, Is.EqualTo(typeof(ThirdDecorator).FullName));
    }
}
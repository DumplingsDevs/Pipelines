using Pipelines.Utils;

namespace Pipelines.Tests.Utils;

[TestFixture]
public class GenericTypeHasClassConstraintTests
{
    [Test]
    public void TestClassConstraint_True()
    {
        bool result = GenericTypeHasClassConstraint.Check(typeof(SomeClass<>).GetGenericArguments()[0]);
        Assert.IsTrue(result);
    }

    [Test]
    public void TestClassConstraint_False()
    {
        bool result = GenericTypeHasClassConstraint.Check(typeof(SomeOtherClass<>).GetGenericArguments()[0]);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestNonGenericType_False()
    {
        bool result = GenericTypeHasClassConstraint.Check(typeof(int));
        Assert.IsFalse(result);
    }

    public class SomeClass<T> where T : class { }
    public class SomeOtherClass<T> where T : new() { }
}

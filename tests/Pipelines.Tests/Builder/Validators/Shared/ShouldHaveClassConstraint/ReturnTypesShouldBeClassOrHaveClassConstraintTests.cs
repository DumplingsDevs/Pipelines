using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;

namespace Pipelines.Tests.Builder.Validators.Shared.ShouldHaveClassConstraint;

[TestFixture]
public class ReturnTypesShouldHaveClassConstraintValidatorTests
{
    private Type _interfaceWithClassConstraint = typeof(IQueryWithClassConstraint<>);
    private Type _interfaceWithoutClassConstraint = typeof(IQueryWithoutClassConstraint<>);
    private Type _interfaceWithClassType = typeof(IQueryWithClass<>);

    [Test]
    public void Validate_ReturnsTypesWithClassConstraint_NoException()
    {
        var types = new List<Type> { _interfaceWithClassConstraint.GetGenericArguments().First() };

        Assert.DoesNotThrow(() =>
            ReturnTypesShouldBeClassOrHaveClassConstraintValidator.Validate(types, _interfaceWithClassConstraint));
    }

    [Test]
    public void Validate_ReturnsTypeClass_NoException()
    {
        var types = new List<Type> { _interfaceWithClassType.GetGenericArguments().First() };

        Assert.DoesNotThrow(() =>
            ReturnTypesShouldBeClassOrHaveClassConstraintValidator.Validate(types, _interfaceWithClassType));
    }

    [Test]
    public void Validate_ReturnsTypesWithoutClassConstraint_Exception()
    {
        var types = new List<Type> { _interfaceWithoutClassConstraint.GetGenericArguments().First() };

        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>(() =>
            ReturnTypesShouldBeClassOrHaveClassConstraintValidator.Validate(types, _interfaceWithoutClassConstraint));
    }

    [Test]
    public void Validate_NonGenericTypes_Exception()
    {
        var types = new List<Type> { typeof(int) };

        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>(() =>
            ReturnTypesShouldBeClassOrHaveClassConstraintValidator.Validate(types, typeof(int)));
    }

    private interface IQueryWithClassConstraint<TResult> where TResult : class
    {
    }

    private interface IQueryWithoutClassConstraint<TResult>
    {
    }

    private interface IQueryWithClass<TResult> where TResult : MyClass
    { }


    private class MyClass
    { }
}
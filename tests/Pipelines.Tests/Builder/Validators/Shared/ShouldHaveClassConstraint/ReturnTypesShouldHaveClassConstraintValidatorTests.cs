using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;

namespace Pipelines.Tests.Builder.Validators.Shared.ShouldHaveClassConstraint;

[TestFixture]
public class ReturnTypesShouldHaveClassConstraintValidatorTests
{
    private Type _interfaceWithClassConstraint = typeof(IQueryWithClassConstraint<>);
    private Type _interfaceWithoutClassConstraint = typeof(IQueryWithoutClassConstraint<>);

    [Test]
    public void Validate_ReturnsTypesWithClassConstraint_NoException()
    {
        List<Type> types = new List<Type> { _interfaceWithClassConstraint.GetGenericArguments().First() };

        Assert.DoesNotThrow(() =>
            ReturnTypesShouldHaveClassConstraintValidator.Validate(types, _interfaceWithClassConstraint));
    }

    [Test]
    public void Validate_ReturnsTypesWithoutClassConstraint_Exception()
    {
        List<Type> types = new List<Type> { _interfaceWithoutClassConstraint.GetGenericArguments().First() };

        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>(() =>
            ReturnTypesShouldHaveClassConstraintValidator.Validate(types, _interfaceWithoutClassConstraint));
    }

    [Test]
    public void Validate_NonGenericTypes_Exception()
    {
        List<Type> types = new List<Type> { typeof(int) };

        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>(() =>
            ReturnTypesShouldHaveClassConstraintValidator.Validate(types, typeof(int)));
    }

    public interface IQueryWithClassConstraint<TResult> where TResult : class { }
    public interface IQueryWithoutClassConstraint<TResult> { }
}
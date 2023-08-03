using Pipelines.Builder.Validators.Input.ResultTypes;
using Pipelines.Builder.Validators.Input.ResultTypes.Exceptions;
using Pipelines.Builder.Validators.Shared.ShouldHaveClassConstraint.Exceptions;

namespace Pipelines.Tests.Builder.Validators.Input;

[TestFixture]
public class ValidateInputReturnTypesTests
{
    private Type _interfaceWithoutGenericArguments = typeof(IQuery);
    private Type _interfaceWithSingleGenericArgument = typeof(IQueryWithSingleGenericArgument<>);
    private Type _interfaceWithMultipleGenericArguments = typeof(IQueryWithMultipleGenericArguments<,>);
    private Type _interfaceWithClassConstraint = typeof(IQueryWithClassConstraint<>);

    [Test]
    public void Validate_InterfaceWithoutGenericArguments_NoException()
    {
        Assert.DoesNotThrow(() =>
            ValidateInputReturnTypes.Validate(_interfaceWithoutGenericArguments));
    }

    [Test]
    public void Validate_InterfaceWithSingleGenericArgument_NoException()
    {
        Assert.Throws<ReturnTypesShouldHaveClassConstraintException>((() =>
            ValidateInputReturnTypes.Validate(_interfaceWithSingleGenericArgument)));
    }

    [Test]
    public void Validate_InterfaceWithMultipleGenericArguments_Exception()
    {
        Assert.Throws<InputTypeShouldNotContainMoreThanOneResultException>(() =>
            ValidateInputReturnTypes.Validate(_interfaceWithMultipleGenericArguments));
    }

    [Test]
    public void Validate_InterfaceWithClassConstraint_NoException()
    {
        Assert.DoesNotThrow(() =>
            ValidateInputReturnTypes.Validate(_interfaceWithClassConstraint));
    }

    private interface IQuery { }

    private interface IQueryWithSingleGenericArgument<TResult> { }

    private interface IQueryWithMultipleGenericArguments<TResult1, TResult2> { }

    private interface IQueryWithClassConstraint<TResult> where TResult : class { }
}
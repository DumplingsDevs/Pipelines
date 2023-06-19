namespace Pipelines.Tests.Builder.Validators.ExactlyOneHandleMethodShouldBeDefineds.Types;

public interface INoMethod<in TCommand, TResult> where TCommand : ICommand<TResult>
{ }
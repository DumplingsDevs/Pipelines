namespace Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod.Types;

public interface INoMethod<in TCommand, TResult> where TCommand : ICommand<TResult>
{ }
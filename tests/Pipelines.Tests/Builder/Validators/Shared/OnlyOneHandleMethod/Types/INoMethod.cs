namespace Pipelines.Tests.Builder.Validators.Shared.OnlyOneHandleMethod.Types;

public interface INoMethod<in TInput, TResult> where TInput : ICommand<TResult>
{ }
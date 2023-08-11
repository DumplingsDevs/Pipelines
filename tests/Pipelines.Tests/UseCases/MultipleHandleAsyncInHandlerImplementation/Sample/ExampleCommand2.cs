using Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Types;

namespace Pipelines.Tests.UseCases.MultipleHandleAsyncInHandlerImplementation.Sample;

public record ExampleCommand2(string Value) : ICommand<ExampleCommandResult>;
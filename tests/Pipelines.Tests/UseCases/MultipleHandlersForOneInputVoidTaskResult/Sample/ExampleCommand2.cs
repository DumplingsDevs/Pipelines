using Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Types;

namespace Pipelines.Tests.UseCases.MultipleHandlersForOneInputVoidTaskResult.Sample;

public record ExampleCommand2(string Value) : ICommand;
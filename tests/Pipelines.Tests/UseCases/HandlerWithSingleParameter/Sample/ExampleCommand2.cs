using Pipelines.Tests.UseCases.HandlerWithSingleParameter.Types;

namespace Pipelines.Tests.UseCases.HandlerWithSingleParameter.Sample;

public record ExampleCommand2(string Value) : ICommand<ExampleCommandResult>;
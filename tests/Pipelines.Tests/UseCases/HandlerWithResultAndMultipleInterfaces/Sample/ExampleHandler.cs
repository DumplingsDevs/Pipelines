using Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndMultipleInterfaces.Sample;

public class ExampleHandler : IHandler<ExampleInput, ExampleCommandResult>, IHandler<ExampleCommand2, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleInput input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }

    public Task<ExampleCommandResult> HandleAsync(ExampleCommand2 input, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(input.Value));
    }
}
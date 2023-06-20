using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleRequest request, CancellationToken token)
    {
        Console.WriteLine($"Handling {nameof(ExampleRequestHandler)} in {nameof(ExampleRequestHandler)}");
        return Task.FromResult(new ExampleCommandResult(request.Value + " Changed"));
    }
}
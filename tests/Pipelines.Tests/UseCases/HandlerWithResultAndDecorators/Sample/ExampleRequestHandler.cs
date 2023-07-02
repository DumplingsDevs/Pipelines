using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample;

public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleRequest request, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(request.Value + " Changed"));
    }
}
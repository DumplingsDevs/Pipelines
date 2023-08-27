using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample;

public class ExampleRequestHandler : IRequestHandler<ExampleRequest, ExampleCommandResult>
{
    public Task<ExampleCommandResult> HandleAsync(ExampleRequest request, CancellationToken token)
    {
        return Task.FromResult(new ExampleCommandResult(request.Value + " Changed"));
    }
}
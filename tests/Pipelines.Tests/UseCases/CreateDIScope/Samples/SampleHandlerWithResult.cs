using Pipelines.Tests.UseCases.CreateDIScope.Types.WithResponse;

namespace Pipelines.Tests.UseCases.CreateDIScope.Samples;

public class SampleHandlerWithResult : IHandlerWithResponse<SampleInputWithResult, SampleResult>
{
    public Task<SampleResult> HandleAsync(SampleInputWithResult input, CancellationToken token)
    {
        return Task.FromResult(new SampleResult());
    }
}
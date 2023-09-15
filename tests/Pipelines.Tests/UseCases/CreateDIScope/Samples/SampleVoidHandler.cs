using Pipelines.Tests.UseCases.CreateDIScope.Types.WithoutResponse;

namespace Pipelines.Tests.UseCases.CreateDIScope.Samples;

public class SampleVoidHandler : IVoidHandler<SampleVoidInput>
{
    public Task HandleAsync(SampleVoidInput command, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}
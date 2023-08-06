using MediatR;

namespace Pipelines.Benchmarks.Sample.Mediator;

public class MediatorExampleRequestHandler : IRequestHandler<MediatorExampleRequest, ExampleCommandResult>
{
    public Task<ExampleCommandResult> Handle(MediatorExampleRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ExampleCommandResult(request.Value + " Changed"));
    }
}
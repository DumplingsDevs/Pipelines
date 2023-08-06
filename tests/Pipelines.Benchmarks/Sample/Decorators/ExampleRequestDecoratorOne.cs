using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class
    ExampleRequestDecoratorOne : IRequestHandler<ExampleRequest,
        ExampleCommandResult>, IDecorator
{
    private readonly IRequestHandler<ExampleRequest, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorOne(
        IRequestHandler<ExampleRequest, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleRequestDecoratorOne));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(ExampleRequestDecoratorOne));

        return result;
    }
}
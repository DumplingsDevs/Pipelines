using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class
    NotSentExampleRequestDecoratorOne : IRequestHandler<NotSentExampleRequest,
        ExampleCommandResult>, IDecorator
{
    private readonly IRequestHandler<NotSentExampleRequest, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public NotSentExampleRequestDecoratorOne(
        IRequestHandler<NotSentExampleRequest, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(NotSentExampleRequest request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(NotSentExampleRequestDecoratorOne));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(NotSentExampleRequestDecoratorOne));

        return result;
    }
}
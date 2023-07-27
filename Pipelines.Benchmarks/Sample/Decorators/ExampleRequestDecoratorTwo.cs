using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class
    ExampleRequestDecoratorTwo : BaseDecorator, IRequestHandler<ExampleRequest,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorTwo(
        IRequestHandler<ExampleRequest, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleRequestDecoratorTwo));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(ExampleRequestDecoratorTwo));

        return result;
    }
}
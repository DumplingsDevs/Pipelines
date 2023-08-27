using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class
    ExampleRequestDecoratorFive : IDecorator, IRequestHandler<ExampleRequest200,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest200, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorFive(
        IRequestHandler<ExampleRequest200, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest200 request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleRequestDecoratorFive));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(ExampleRequestDecoratorFive));

        return result;
    }
}
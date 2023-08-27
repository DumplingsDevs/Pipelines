using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

[Decorator]
public class
    ExampleRequestDecoratorThree : IRequestHandler<ExampleRequest200,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest200, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorThree(
        IRequestHandler<ExampleRequest200, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest200 request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleRequestDecoratorThree));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(ExampleRequestDecoratorThree));

        return result;
    }
}
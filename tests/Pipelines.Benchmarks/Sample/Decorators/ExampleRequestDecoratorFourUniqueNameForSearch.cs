using Pipelines.Benchmarks.Types;

namespace Pipelines.Benchmarks.Sample.Decorators;

public class
    ExampleRequestDecoratorFourUniqueNameForSearch : IRequestHandler<ExampleRequest200,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest200, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestDecoratorFourUniqueNameForSearch(
        IRequestHandler<ExampleRequest200, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest200 request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(ExampleRequestDecoratorFourUniqueNameForSearch));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(ExampleRequestDecoratorFourUniqueNameForSearch));

        return result;
    }
}
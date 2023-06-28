using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class
    FirstRequestDecorator : IRequestHandler<ExampleRequest,
        ExampleCommandResult>, IDecorator
{
    private readonly IRequestHandler<ExampleRequest, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public FirstRequestDecorator(
        IRequestHandler<ExampleRequest, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(FirstRequestDecorator));

        var result = await _handler.HandleAsync(request, token);

        _state.Status.Add(nameof(FirstRequestDecorator));

        return result;
    }
}
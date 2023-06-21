using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class
    ExampleRequestValidator : IRequestHandler<ExampleRequest,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest, ExampleCommandResult> _handler;
    private readonly DecoratorsState _state;

    public ExampleRequestValidator(
        IRequestHandler<ExampleRequest, ExampleCommandResult> handler, DecoratorsState state)
    {
        _handler = handler;
        _state = state;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        _state.Status.Add("ExampleRequestValidator");

        Console.WriteLine($"[ExampleRequestValidator] Start of handling {request.GetType().Name}");
        var result = await _handler.HandleAsync(request, token);
        Console.WriteLine($"[ExampleRequestValidator] Stop of handling {request.GetType().Name}");

        _state.Status.Add("ExampleRequestValidator");

        return result;
    }
}
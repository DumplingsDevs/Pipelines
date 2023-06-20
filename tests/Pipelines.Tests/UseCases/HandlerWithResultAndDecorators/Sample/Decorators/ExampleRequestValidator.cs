using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class
    ExampleRequestValidator : IRequestHandler<ExampleRequest,
        ExampleCommandResult>
{
    private readonly IRequestHandler<ExampleRequest, ExampleCommandResult> _handler;

    public ExampleRequestValidator(
        IRequestHandler<ExampleRequest, ExampleCommandResult> handler)
    {
        _handler = handler;
    }

    public async Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        Console.WriteLine($"[ExampleRequestValidator] Start of handling {request.GetType().Name}");
        var result = await _handler.HandleAsync(request, token);
        Console.WriteLine($"[ExampleRequestValidator] Stop of handling {request.GetType().Name}");

        return result;
    }
}
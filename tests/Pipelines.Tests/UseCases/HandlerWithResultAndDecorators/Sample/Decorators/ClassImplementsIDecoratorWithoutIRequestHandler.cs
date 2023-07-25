using Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Types;

namespace Pipelines.Tests.UseCases.HandlerWithResultAndDecorators.Sample.Decorators;

public class
    ClassImplementsIDecoratorWithoutIRequestHandler : IDecorator
{
    private readonly DecoratorsState _state;

    public ClassImplementsIDecoratorWithoutIRequestHandler(DecoratorsState state)
    {
        _state = state;
    }

    public Task<ExampleCommandResult> HandleAsync(ExampleRequest request,
        CancellationToken token)
    {
        _state.Status.Add(nameof(ClassImplementsIDecoratorWithoutIRequestHandler));


        _state.Status.Add(nameof(ClassImplementsIDecoratorWithoutIRequestHandler));

        return Task.FromResult(new ExampleCommandResult("test"));
    }
}
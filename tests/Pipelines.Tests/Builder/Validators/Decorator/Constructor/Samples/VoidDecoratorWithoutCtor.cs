namespace Pipelines.Tests.Builder.Validators.Decorator.Constructor.Samples;
using Types;

public class VoidDecoratorWithoutCtor: IVoidCommandHandler<VoidCommand>
{
    public async Task HandleAsync(VoidCommand command, CancellationToken token)
    {
    }
}
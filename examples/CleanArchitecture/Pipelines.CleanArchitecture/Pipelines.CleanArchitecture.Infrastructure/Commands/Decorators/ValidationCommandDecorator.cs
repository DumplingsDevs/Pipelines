using Pipelines.CleanArchitecture.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.CleanArchitecture.Infrastructure.Commands.Decorators;

public class ValidationCommandDecorator<TCommand> : ICommandHandler<TCommand> where TCommand: ICommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandHandler<TCommand> _commandHandler;

    public ValidationCommandDecorator(IServiceProvider serviceProvider, ICommandHandler<TCommand> commandHandler)
    {
        _serviceProvider = serviceProvider;
        _commandHandler = commandHandler;
    }

    public async Task<Guid> HandleAsync(TCommand command, CancellationToken token)
    {
        await ValidateWithBusinessValidator(command, token);

        return await _commandHandler.HandleAsync(command,token).ConfigureAwait(false);
    }

    private async Task ValidateWithBusinessValidator(TCommand command, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<ICommandValidator<TCommand>>();

        if (validator != null)
        {
            await validator.ValidateAsync(command, cancellationToken);
        }
    }
}
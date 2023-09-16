using FluentValidation;
using FluentValidation.Results;
using Pipelines.CleanArchitecture.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.Commands.Exceptions;
using Pipelines.CleanArchitecture.Abstractions.Errors;

namespace Pipelines.CleanArchitecture.Infrastructure.Commands.Decorators;

internal class FluentValidationCommandDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandHandler<TCommand> _commandHandler;

    public FluentValidationCommandDecorator(IServiceProvider serviceProvider, ICommandHandler<TCommand> commandHandler)
    {
        _serviceProvider = serviceProvider;
        _commandHandler = commandHandler;
    }

    public async Task<Guid> HandleAsync(TCommand command, CancellationToken token)
    {
        {
            var structValidationResult = await ValidateAsync(_serviceProvider, command, token);

            if (!structValidationResult.Any())
            {
                return await _commandHandler.HandleAsync(command, token).ConfigureAwait(false);
            }

            var errorDetails = structValidationResult
                .Select(x => new DetailedErrorDescription(x.ErrorCode, x.ErrorMessage, x.PropertyName))
                .ToList();

            throw new CommandValidationFailedException(errorDetails);
        }
    }

    private async Task<List<ValidationFailure>> ValidateAsync(IServiceProvider provider, TCommand command,
        CancellationToken cancellationToken)
    {
        var validator = provider.GetService<IValidator<TCommand>>();
        if (validator is null)
        {
            return new List<ValidationFailure>();
        }

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult.Errors;
    }
}
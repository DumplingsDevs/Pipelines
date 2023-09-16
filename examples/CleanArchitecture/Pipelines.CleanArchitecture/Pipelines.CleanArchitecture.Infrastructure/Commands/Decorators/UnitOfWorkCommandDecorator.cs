using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.Commands;
using Pipelines.CleanArchitecture.Infrastructure.Persistance;

namespace Pipelines.CleanArchitecture.Infrastructure.Commands.Decorators;

internal class UnitOfWorkCommandDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ToDoDbContext _dbContext;
    private readonly ICommandHandler<TCommand> _commandHandler;

    public UnitOfWorkCommandDecorator(IServiceProvider serviceProvider, ToDoDbContext dbContext,
        ICommandHandler<TCommand> commandHandler)
    {
        _dbContext = dbContext;
        _commandHandler = commandHandler;
    }

    public async Task<Guid> HandleAsync(TCommand command, CancellationToken token)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
        var result = await _commandHandler.HandleAsync(command, token).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        return result;
    }
}
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Domain.Repositories;
using Pipelines.CleanArchitecture.Infrastructure.Persistance.Repositories;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance;

public static class Extensions
{
    private static readonly string ConnectionString = "Data Source=Pipelines;Mode=Memory;Cache=Shared";
    private static DbConnection? _dbConnection;

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        _dbConnection = CreateInMemoryDatabaseConnection();
        
        services.AddScoped<IToDoRepository, ToDoRepository>();
        services.AddDbContext<ToDoDbContext>(options =>
            {
                options.UseSqlite(_dbConnection);
                options.ConfigureWarnings(x =>
                    x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));
            }
        );

        return services;
    }
    
    private static DbConnection? CreateInMemoryDatabaseConnection()
    {
        var connectionString = string.Format(ConnectionString, Guid.NewGuid());
        
        var connection = new SqliteConnection(connectionString);
        connection.OpenAsync().GetAwaiter().GetResult();
        return connection;
    }
}
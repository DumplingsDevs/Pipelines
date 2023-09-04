using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance;

public class DatabaseCreator
{
    public static void CreateDatabaseSchema(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
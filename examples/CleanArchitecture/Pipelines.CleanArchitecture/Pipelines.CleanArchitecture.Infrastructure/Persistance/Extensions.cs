using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Domain.Repositories;
using Pipelines.CleanArchitecture.Infrastructure.Persistance.Repositories;

namespace Pipelines.CleanArchitecture.Infrastructure.Persistance;

public static class Extensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IToDoRepository, ToDoRepository>();
        services.AddDbContext<ToDoDbContext>(options => options.UseInMemoryDatabase("ToDoDatabase"));
        
        return services;
    }
}
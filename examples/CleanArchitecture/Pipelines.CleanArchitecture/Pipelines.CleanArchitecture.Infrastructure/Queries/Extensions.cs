using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.CleanArchitecture.Abstractions.Queries;
using Pipelines.CleanArchitecture.Application;
using Pipelines.CleanArchitecture.Infrastructure.Queries.Decorators;

namespace Pipelines.CleanArchitecture.Infrastructure.Queries;

public static class Extensions
{
    public static void AddQueries(this IServiceCollection services)
    {
        var infrastructureAssembly = typeof(Extensions).Assembly;
        var queryAssembly = typeof(ApplicationMarker).Assembly;

        services.AddPipeline()
            .AddInput(typeof(IQuery<>))
            .AddHandler(typeof(IQueryHandler<,>), infrastructureAssembly)
            .AddDispatcher<IQueryDispatcher>(infrastructureAssembly)
            .WithDecorator(typeof(FluentValidationQueryDecorator<,>))
            .Build();
        
        services.Scan(s => s.FromAssemblies(queryAssembly)
            .AddClasses(classes =>
                classes.AssignableTo(typeof(IValidator<>)).Where(_ => !_.IsGenericType))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}
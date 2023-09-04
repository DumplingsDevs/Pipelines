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
            .AddHandler(typeof(IQueryHandler<,>), queryAssembly)
            .AddDispatcher<IQueryDispatcher>(infrastructureAssembly)
            .WithDecorator(typeof(FluentValidationQueryDecorator<,>))
            .Build();
    }
}
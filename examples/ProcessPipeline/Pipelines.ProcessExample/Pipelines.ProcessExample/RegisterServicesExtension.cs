using Pipelines.ProcessExample.Services.DiscountCodes;
using Pipelines.ProcessExample.Services.Emails;
using Pipelines.ProcessExample.Services.LoyaltyDiscount;
using Pipelines.ProcessExample.Services.Products;
using Pipelines.ProcessExample.Services.Repositories.Orders;
using Pipelines.ProcessExample.Services.Stock;
using Pipelines.ProcessExample.Services.Users;

namespace Pipelines.ProcessExample;

public static class RegisterServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGetCodeDiscount, GetCodeDiscount>();
        services.AddScoped<IEmailScheduler, EmailScheduler>();
        services.AddScoped<IGetLoyaltyDiscount, GetLoyaltyDiscount>();
        services.AddScoped<IGetProducts, GetProducts>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IUserContext, UserContext>();
    }
}
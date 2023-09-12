using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.Stock;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.Process;

[OrderStep(1)]
public class CheckIfProductIsInStock : IPlaceOrderHandler<PlaceOrder>
{
    private readonly IPlaceOrderHandler<PlaceOrder> _handler;
    private readonly IStockService _stockService;

    public CheckIfProductIsInStock(IPlaceOrderHandler<PlaceOrder> handler, IStockService stockService)
    {
        _handler = handler;
        _stockService = stockService;
    }

    public async Task<Order> HandleAsync(PlaceOrder command, List<Discount> discounts, CancellationToken token)
    {
        foreach (var productDto in command.Products)
        {
            if (!_stockService.CheckIfInStock(productDto.Id, productDto.Quantity))
            {
                throw new Exception($"Product with Id {productDto.Id} is out of stock");
            }
        }

        return await _handler.HandleAsync(command, discounts, token);
    }
}
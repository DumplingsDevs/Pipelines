using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.Stock;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Process;

[OrderStepV2(1)]
public class CheckIfProductIsInStockV2 : IPlaceOrderV2Handler<PlaceOrderV2>
{
    private readonly IPlaceOrderV2Handler<PlaceOrderV2> _handler;
    private readonly IStockService _stockService;

    public CheckIfProductIsInStockV2(IPlaceOrderV2Handler<PlaceOrderV2> handler, IStockService stockService)
    {
        _handler = handler;
        _stockService = stockService;
    }

    public async Task<Order> HandleAsync(PlaceOrderV2 command, CancellationToken token)
    {
        foreach (var productDto in command.Products)
        {
            if (!_stockService.CheckIfInStock(productDto.Id, productDto.Quantity))
            {
                throw new Exception($"Product with Id {productDto.Id} is out of stock");
            }
        }

        return await _handler.HandleAsync(command, token);
    }
}
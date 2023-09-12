using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.Products;
using Pipelines.ProcessExample.Services.Repositories.Orders;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Process;

public class CreateOrderV2 : IPlaceOrderV2Handler<PlaceOrderV2>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IGetProducts _getProducts;
    private readonly DiscountState _discountState;

    public CreateOrderV2(IOrderRepository orderRepository, IGetProducts getProducts, DiscountState discountState)
    {
        _orderRepository = orderRepository;
        _getProducts = getProducts;
        _discountState = discountState;
    }

    public async Task<Order> HandleAsync(PlaceOrderV2 command, CancellationToken token)
    {
        var productsIds = command.Products.Select(x => x.Id).ToList();
        var products = _getProducts.Get(productsIds);

        var order = new Order(products, _discountState.Discounts.ToList());
        await _orderRepository.AddAsync(order, token);

        return order;
    }
}
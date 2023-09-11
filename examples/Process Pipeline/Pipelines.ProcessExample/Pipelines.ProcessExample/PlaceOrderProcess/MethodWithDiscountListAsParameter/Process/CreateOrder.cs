using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.Products;
using Pipelines.ProcessExample.Services.Repositories.Orders;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.Process;

public class CreateOrder : IPlaceOrderHandler<PlaceOrder>
{
    private IOrderRepository _orderRepository;
    private IGetProducts _getProducts;

    public CreateOrder(IOrderRepository orderRepository, IGetProducts getProducts)
    {
        _orderRepository = orderRepository;
        _getProducts = getProducts;
    }

    public async Task<Order> HandleAsync(PlaceOrder command, List<Discount> discounts, CancellationToken token)
    {
        var productsIds = command.Products.Select(x => x.Id).ToList();
        var products = _getProducts.Get(productsIds);

        var order = new Order(products, discounts);
        await _orderRepository.AddAsync(order, token);

        return order;
    }
}
using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.DiscountCodes;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.Process;

[OrderStep(2)]
public class ApplyDiscountCode : IPlaceOrderHandler<PlaceOrder>
{
    private readonly IPlaceOrderHandler<PlaceOrder> _handler;
    private readonly IGetCodeDiscount _getCodeDiscount;

    public ApplyDiscountCode(IPlaceOrderHandler<PlaceOrder> handler, IGetCodeDiscount getCodeDiscount)
    {
        _handler = handler;
        _getCodeDiscount = getCodeDiscount;
    }

    public async Task<Order> HandleAsync(PlaceOrder command, List<Discount> discounts, CancellationToken token)
    {
        var discount = _getCodeDiscount.GetByCode(command.DiscountCode);
        if (discount != null)
        {
            discounts.Add(discount);
        }

        return await _handler.HandleAsync(command, discounts, token);
    }
}
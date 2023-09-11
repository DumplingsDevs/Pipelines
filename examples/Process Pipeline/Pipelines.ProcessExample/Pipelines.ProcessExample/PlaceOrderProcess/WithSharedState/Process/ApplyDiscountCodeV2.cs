using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.DiscountCodes;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Process;

[OrderStepV2(2)]
public class ApplyDiscountCodeV2 : IPlaceOrderV2Handler<PlaceOrderV2>
{
    private readonly IPlaceOrderV2Handler<PlaceOrderV2> _handler;
    private readonly IGetCodeDiscount _getCodeDiscount;
    private readonly DiscountState _discountState;

    public ApplyDiscountCodeV2(IPlaceOrderV2Handler<PlaceOrderV2> handler, IGetCodeDiscount getCodeDiscount, DiscountState discountState)
    {
        _handler = handler;
        _getCodeDiscount = getCodeDiscount;
        _discountState = discountState;
    }

    public async Task<Order> HandleAsync(PlaceOrderV2 command, CancellationToken token)
    {
        var discount = _getCodeDiscount.GetByCode(command.DiscountCode);
        if (discount != null)
        {
            _discountState.Add(discount);
        }

        return await _handler.HandleAsync(command, token);
    }
}
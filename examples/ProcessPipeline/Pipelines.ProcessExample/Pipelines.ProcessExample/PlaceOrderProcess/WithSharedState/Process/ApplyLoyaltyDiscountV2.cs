using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.LoyaltyDiscount;
using Pipelines.ProcessExample.Services.Users;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Process;

[OrderStepV2(3)]
public class ApplyLoyaltyDiscountV2 : IPlaceOrderV2Handler<PlaceOrderV2>
{
    private readonly IPlaceOrderV2Handler<PlaceOrderV2> _handler;
    private readonly IGetLoyaltyDiscount _getLoyaltyDiscount;
    private readonly IUserContext _userContext;
    private readonly DiscountState _discountState;

    public ApplyLoyaltyDiscountV2(IPlaceOrderV2Handler<PlaceOrderV2> handler, IGetLoyaltyDiscount getLoyaltyDiscount, IUserContext userContext, DiscountState discountState)
    {
        _handler = handler;
        _getLoyaltyDiscount = getLoyaltyDiscount;
        _userContext = userContext;
        _discountState = discountState;
    }
    
    public async Task<Order> HandleAsync(PlaceOrderV2 command, CancellationToken token)
    {
        var discount = _getLoyaltyDiscount.GetByUserId(_userContext.GetUser());
        if (discount != null)
        {
            _discountState.Add(discount);
        }
        
        return await _handler.HandleAsync(command, token);
    }
}
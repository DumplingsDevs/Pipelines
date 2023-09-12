using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.LoyaltyDiscount;
using Pipelines.ProcessExample.Services.Users;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.Process;

[OrderStep(3)]
public class ApplyLoyaltyDiscount : IPlaceOrderHandler<PlaceOrder>
{
    private readonly IPlaceOrderHandler<PlaceOrder> _handler;
    private readonly IGetLoyaltyDiscount _getLoyaltyDiscount;
    private readonly IUserContext _userContext;
    
    public ApplyLoyaltyDiscount(IPlaceOrderHandler<PlaceOrder> handler, IGetLoyaltyDiscount getLoyaltyDiscount, IUserContext userContext)
    {
        _handler = handler;
        _getLoyaltyDiscount = getLoyaltyDiscount;
        _userContext = userContext;
    }
    
    public async Task<Order> HandleAsync(PlaceOrder command, List<Discount> discounts, CancellationToken token)
    {
        var discount = _getLoyaltyDiscount.GetByUserId(_userContext.GetUser());
        if (discount != null)
        {
            discounts.Add(discount);
        }
        
        return await _handler.HandleAsync(command, discounts, token);
    }
}
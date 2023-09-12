using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.LoyaltyDiscount;

public class GetLoyaltyDiscount : IGetLoyaltyDiscount
{
    public Discount? GetByUserId(Guid UserId)
    {
        return null;
    }
}
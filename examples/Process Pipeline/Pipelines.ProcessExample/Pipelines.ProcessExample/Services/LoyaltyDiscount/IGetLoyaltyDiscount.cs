using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.LoyaltyDiscount;

public interface IGetLoyaltyDiscount
{
    public Discount? GetByUserId(Guid UserId);
}
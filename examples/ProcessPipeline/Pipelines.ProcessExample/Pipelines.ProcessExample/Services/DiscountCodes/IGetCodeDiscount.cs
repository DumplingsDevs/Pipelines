using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.DiscountCodes;

public interface IGetCodeDiscount
{
    public Discount? GetByCode(string code);
}
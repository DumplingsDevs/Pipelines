using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.Services.DiscountCodes;

public class GetCodeDiscount : IGetCodeDiscount
{
    public Discount? GetByCode(string code)
    {
        return null;
    }
}
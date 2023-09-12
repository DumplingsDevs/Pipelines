using Pipelines.ProcessExample.Entities;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState;

public class DiscountState
{
    private List<Discount> _discounts;
    public IReadOnlyList<Discount> Discounts => _discounts.AsReadOnly();

    public DiscountState()
    {
        _discounts = new List<Discount>();
    }

    public void Add(Discount discount)
    {
        _discounts.Add(discount);
    }
}
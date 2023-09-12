using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.Emails;

namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.Process;

[OrderStep(4)]
public class SendEmailToCustomer : IPlaceOrderHandler<PlaceOrder>
{
    private readonly IPlaceOrderHandler<PlaceOrder> _handler;
    private readonly IEmailScheduler _emailScheduler;
    
    public SendEmailToCustomer(IPlaceOrderHandler<PlaceOrder> handler, IEmailScheduler emailScheduler)
    {
        _handler = handler;
        _emailScheduler = emailScheduler;
    }

    public async Task<Order> HandleAsync(PlaceOrder command, List<Discount> discounts, CancellationToken token)
    {
        var order = await _handler.HandleAsync(command, discounts, token);
        var content = CreateContent(order);
        
        await _emailScheduler.Schedule(content,token);
        
        return order;
    }

    private string CreateContent(Order order)
    {
        return "Fake email content based on product list";
    }
}
using Pipelines.ProcessExample.Entities;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;
using Pipelines.ProcessExample.Services.Emails;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Process;

[OrderStepV2(4)]
public class SendEmailToCustomerV2 : IPlaceOrderV2Handler<PlaceOrderV2>
{
    private readonly IPlaceOrderV2Handler<PlaceOrderV2> _handler;
    private readonly IEmailScheduler _emailScheduler;
    
    public SendEmailToCustomerV2(IPlaceOrderV2Handler<PlaceOrderV2> handler, IEmailScheduler emailScheduler)
    {
        _handler = handler;
        _emailScheduler = emailScheduler;
    }

    public async Task<Order> HandleAsync(PlaceOrderV2 command, CancellationToken token)
    {
        var order = await _handler.HandleAsync(command, token);
        var content = CreateContent(order);
        
        await _emailScheduler.Schedule(content,token);
        
        return order;
    }

    private string CreateContent(Order order)
    {
        return "Fake email content based on product list";
    }
}
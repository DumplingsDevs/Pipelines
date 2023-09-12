namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

public class OrderStepV2 : Attribute
{
    public OrderStepV2(int stepNumber)
    {
        StepNumber = stepNumber;
    }

    public int StepNumber { get; }
}
namespace Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter.PipelinesInterfaces;

public class OrderStep : Attribute
{
    public OrderStep(int stepNumber)
    {
        StepNumber = stepNumber;
    }

    public int StepNumber { get; }
}
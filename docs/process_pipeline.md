# Implementing a Multistep Process with Pipelines

-----

Imagine you want implement make an order feature. But it's not so simple because:

- You must ensure the product is still in stock.
- If there's a discount code, it should be applied.
- Loyal customers deserve their loyalty discount.
- After placing the order, a confirmation email should be sent.

`Pipelines` provide a streamlined approach to craft a dedicated process for this scenario, allowing you to dictate the sequence of individual process steps.

![Pipelines process](./img/place_order_process.png)

Based on order process requirements, it can be structured as follows:
•	Decorator: Check if product is in stock
•	Decorator: Apply discount code
•	Decorator: Apply loyalty discount
•	Handler: Create order
•	Decorator: Send email to customer

Discounts are created separately, not where you create the order. To use these discounts when you create an order, you have two options:

1. You can pass the list of discounts as a method parameter when you're making the order.
   - [See code here](../examples/ProcessPipeline/Pipelines.ProcessExample/Pipelines.ProcessExample/PlaceOrderProcess/MethodWithDiscountListAsParameter/)

2. You can use a shared object that's available for a single order. This way, the handler responsible for creating the order can access the discounts.
   - [See code here](../examples/ProcessPipeline/Pipelines.ProcessExample/Pipelines.ProcessExample/PlaceOrderProcess/WithSharedState/)

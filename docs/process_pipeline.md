# Multistep process with Pipelines

-----

Imagine that you need to implement ordering functionality. However, this is not a trivial task because:
- you need to make sure whether the product is still available
- add a discount if a discount code was provided
- add a discount if the customer is eligible for a loyalty discount
- after successfully placing the order, you should receive an email confirming the order

Pipelines will allow you to easily create a dedicated pipeline for this process, in which you will be able to control the order in which individual process steps are invoked.

![Pipelines process](./img/place_order_process.png)

In this case, discounts will be created outside the place where the order is created.

To transfer the created discounts to the handler who will be responsible for creating the order, you can do it in two ways:

- pass the list of created discounts as a method parameter
- take advantage of the fact that a scope is created for each call to pipelines, so you can use an object that will be shared for a single call


List of discounts as method parameter example: 
[Click here to see code example](../examples/ProcessPipeline/Pipelines.ProcessExample/Pipelines.ProcessExample/PlaceOrderProcess/MethodWithDiscountListAsParameter/)

List of discounts in shared object

[Click here to see code example](../examples/ProcessPipeline/Pipelines.ProcessExample/Pipelines.ProcessExample/PlaceOrderProcess/WithSharedState/)
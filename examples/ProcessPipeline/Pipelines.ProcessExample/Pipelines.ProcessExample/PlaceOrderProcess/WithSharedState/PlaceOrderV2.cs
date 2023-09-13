using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.Dtos;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState.PipelinesInterfaces;

namespace Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState;

public record PlaceOrderV2(List<ProductDtoV2> Products, string DiscountCode) : IPlaceOrderV2Input;
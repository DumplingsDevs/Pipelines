using Pipelines.ProcessExample;
using Pipelines.ProcessExample.Extensions;
using Pipelines.ProcessExample.PlaceOrderProcess.MethodWithDiscountListAsParameter;
using Pipelines.ProcessExample.PlaceOrderProcess.WithSharedState;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices();
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var processAssembly = typeof(Program).Assembly;

builder.Services.AddPipelineWithDiscountAsMethodParameter(processAssembly);
builder.Services.AddPipelineWithSharedState(processAssembly);

var app = builder.Build();

app.RegisterEndpoints();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Pipelines Example");
    c.RoutePrefix = string.Empty;
});

app.Run();
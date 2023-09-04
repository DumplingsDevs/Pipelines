using Pipelines.CleanArchitecture.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Pipelines Example");
    c.RoutePrefix = string.Empty;
});

app.RegisterEndpoints();
app.Run();
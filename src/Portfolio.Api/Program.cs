using Portfolio.Business.Utils;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument((options, _) =>
{
    options.Title = "Portfolio Api";
    options.Version = "v1";
    options.DocumentName = "v1";
});


builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseOpenApi(options => { options.Path = "/openapi/{documentName}.json"; });
app.MapScalarApiReference();

app.MapControllers();

app.Run();
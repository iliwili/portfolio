using Brevo.Client;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mediator;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Errors;
using Portfolio.Business;
using Portfolio.Business.Auth.Helpers;
using Portfolio.Business.Auth.Services;
using Portfolio.Business.Configuration;
using Portfolio.Business.Emails.Services;
using Portfolio.Business.Pipeline;
using Portfolio.Dal;
using Portfolio.Dal.Utils;
using Portfolio.Utils;
using Scalar.AspNetCore;
using Task = System.Threading.Tasks.Task;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UrlOptions>(builder.Configuration.GetSection(UrlOptions.SectionName));

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    options.UseNpgsql(cs);
    // Temporarily suppress pending migration warning to allow build
    options.ConfigureWarnings(warnings =>
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddHttpContextAccessor();

// Fluent validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<IBusiness>();
builder.Services.AddValidatorsFromAssemblyContaining<IDal>();

// Register utilities
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Dashboard", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ISecureTokenGenerator, SecureTokenGenerator>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "portfolio_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument((options, _) =>
{
    options.Title = "Portfolio Api";
    options.Version = "v1";
    options.DocumentName = "v1";
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Transient; });
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddBrevo(options =>
{
    options.BaseUrl = builder.Configuration.GetValue<string>("Brevo:BaseUrl") ?? throw new InvalidOperationException("Brevo:BaseUrl is not configured.");
    options.ApiKey = builder.Configuration.GetValue<string>("Brevo:ApiKey") ?? throw new InvalidOperationException("Brevo:ApiKey is not configured.");
});
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.UseCors("Dashboard");

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();

app.UseOpenApi(options => { options.Path = "/openapi/{documentName}.json"; });
app.MapScalarApiReference();

app.MapControllers();

app.Run();
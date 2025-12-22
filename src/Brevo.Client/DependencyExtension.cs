using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Brevo.Client;

public class BrevoOptions
{
    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; }
}

public static class DependencyExtension
{
    public static IServiceCollection AddBrevo(this IServiceCollection services, Action<BrevoOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);

        services.AddTransient<ApiTokenHandler>();

        services
            .AddHttpClient("brevo-api")
            .AddHttpMessageHandler<ApiTokenHandler>();

        services.AddTransient<IBrevoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<BrevoOptions>>().Value;
            var httpFactory = sp.GetRequiredService<IHttpClientFactory>();
            var http = httpFactory.CreateClient("brevo-api");

            var client = new BrevoClient(http)
            {
                BaseUrl = options.BaseUrl
            };

            return client;
        });

        return services;
    }
}
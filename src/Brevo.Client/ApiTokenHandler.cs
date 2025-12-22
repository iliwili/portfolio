using Microsoft.Extensions.Options;

namespace Brevo.Client;

public sealed class ApiTokenHandler(IOptions<BrevoOptions> brevoOptions) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        request.Headers.TryAddWithoutValidation("api-key", brevoOptions.Value.ApiKey);
        return await base.SendAsync(request, ct);
    }
}
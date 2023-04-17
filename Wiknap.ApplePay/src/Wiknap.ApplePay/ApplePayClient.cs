using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Wiknap.ApplePay.Model;

namespace Wiknap.ApplePay;

public class ApplePayClient : IApplePayClient
{
    private readonly HttpClient httpClient;

    public ApplePayClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<JsonDocument> GetMerchantSessionAsync(
        string requestUri,
        MerchantSessionRequest request,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(request);
        using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var response = await httpClient.PostAsync(requestUri, content, cancellationToken);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
    }
}

public interface IApplePayClient
{
    Task<JsonDocument> GetMerchantSessionAsync(string requestUri, MerchantSessionRequest request,
        CancellationToken cancellationToken = default);
}


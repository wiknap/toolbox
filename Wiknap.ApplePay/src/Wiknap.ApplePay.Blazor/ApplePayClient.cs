using System.Net.Http.Json;
using System.Text.Json;

namespace Wiknap.ApplePay.Blazor;

public class ApplePayClient : IApplePayClient
{
    private readonly HttpClient httpClient;

    public ApplePayClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<JsonElement> ValidateMerchantAsync(string validationUrl, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/applePay/session/start", validationUrl, cancellationToken: ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        return (await JsonDocument.ParseAsync(stream, cancellationToken: ct)).RootElement;
    }

    public async Task<bool> AuthorizePaymentAsync(string token, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/applePay/payment", token, cancellationToken: ct);
        return response.IsSuccessStatusCode;
    }
}

public interface IApplePayClient
{
    Task<JsonElement> ValidateMerchantAsync(string validationUrl, CancellationToken ct = default);
    Task<bool> AuthorizePaymentAsync(string token, CancellationToken ct = default);
}
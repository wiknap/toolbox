using System.Net.Http.Json;

namespace Wiknap.ApplePay.Adyen;

public class AdyenClient : IAdyenClient
{
    private readonly HttpClient httpClient;

    public AdyenClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<bool> PostPaymentAsync(PaymentRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("https://checkout-test.adyen.com/v70/payments", request, cancellationToken: ct);
        return response.IsSuccessStatusCode;
    }
}

public interface IAdyenClient
{
    Task<bool> PostPaymentAsync(PaymentRequest request, CancellationToken ct = default);
}
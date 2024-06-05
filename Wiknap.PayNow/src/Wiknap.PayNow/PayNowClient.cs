using System.Net.Mime;
using System.Text;
using System.Text.Json;

using JetBrains.Annotations;

using Wiknap.PayNow.Exceptions;
using Wiknap.PayNow.Model;
using Wiknap.PayNow.Path;

namespace Wiknap.PayNow;

[PublicAPI]
public sealed class PayNowClient : IPayNowClient, IDisposable
{
    private readonly Encoding encoding = Encoding.UTF8;
    private readonly HmacSha256Calculator hmacSha256Calculator;
    private readonly HttpClient httpClient;

    public PayNowClient(HttpClient httpClient, IPayNowConfiguration configuration)
    {
        this.httpClient = httpClient;

        this.httpClient.BaseAddress = new Uri(configuration.ApiPath);
        this.httpClient.DefaultRequestHeaders.Add(PayNowConstants.HeadersNames.ApiKey, configuration.ApiKey);
        hmacSha256Calculator = new HmacSha256Calculator(encoding.GetBytes(configuration.SignatureKey));
    }

    public async Task<PostPaymentResponse> PostPaymentRequestAsync(PostPaymentRequest paymentRequest,
        CancellationToken ct = default)
    {
        var builder = GetBuilder();
        builder.AddPaymentsPath();

        var response = await SendAndDeserializeAsync<PostPaymentResponse>(
            HttpMethod.Post,
            builder.ToString(),
            paymentRequest, ct).ConfigureAwait(false);

        return response ?? throw new EmptyResponseException();
    }

    public async Task<GetPaymentStatusResponse> GetPaymentStatusAsync(string paymentId, CancellationToken ct = default)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentPath(paymentId)
            .AddPaymentStatusPath();

        var response =
            await SendAndDeserializeAsync<GetPaymentStatusResponse>(HttpMethod.Get, builder.ToString(), ct: ct)
                .ConfigureAwait(false);

        return response ?? throw new EmptyResponseException();
    }

    public async Task<GetPaymentMethodsResponse> GetPaymentMethodsAsync(Currency currency = Currency.PLN,
        CancellationToken ct = default)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentMethodsPath(currency);

        var response =
            await SendAndDeserializeAsync<GetPaymentMethodsResponse>(HttpMethod.Get, builder.ToString(), ct: ct)
                .ConfigureAwait(false);

        return response ?? throw new EmptyResponseException();
    }

    public async Task<PostRefundResponse> PostRefundRequestAsync(string paymentId, PostRefundRequest refundRequest,
        CancellationToken ct = default)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentPath(paymentId)
            .AddPaymentRefundsPath();

        var response = await SendAndDeserializeAsync<PostRefundResponse>(
            HttpMethod.Post,
            builder.ToString(),
            refundRequest,
            ct).ConfigureAwait(false);

        return response ?? throw new EmptyResponseException();
    }

    public async Task<GetRefundStatusResponse> GetRefundStatusAsync(string refundId, CancellationToken ct = default)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentRefundsPath()
            .AddRefundPath(refundId)
            .AddRefundStatusPath();

        var response =
            await SendAndDeserializeAsync<GetRefundStatusResponse>(HttpMethod.Get, builder.ToString(), ct: ct)
                .ConfigureAwait(false);

        return response ?? throw new EmptyResponseException();
    }

    private static PayNowApiPathBuilder GetBuilder() => new();

    private async Task<T?> SendAndDeserializeAsync<T>(HttpMethod method, string path, object? content = null,
        CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(method, path);

        if (content != null)
        {
            var serializedContent = JsonSerializer.Serialize(content);
            request.Content = new StringContent(serializedContent, encoding, MediaTypeNames.Application.Json);

            request.Headers.Add(PayNowConstants.HeadersNames.Signature,
                hmacSha256Calculator.CalculateHmac(encoding.GetBytes(serializedContent)));
        }

        request.Headers.Add(PayNowConstants.HeadersNames.IdempotencyKey, Guid.NewGuid().ToString());

        using var response = await httpClient.SendAsync(request, ct).ConfigureAwait(false);
        await using var contentStream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<T>(contentStream, cancellationToken: ct).ConfigureAwait(false);
    }

    public void Dispose() => hmacSha256Calculator.Dispose();
}

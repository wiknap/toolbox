using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wiknap.PayNow.Exceptions;
using Wiknap.PayNow.Model;
using Wiknap.PayNow.Path;

namespace Wiknap.PayNow;

public sealed class PayNowClient : IPayNowClient
{
    private readonly Encoding encoding = Encoding.UTF8;
    private readonly HmacSha256Calculator hmacSha256Calculator;
    private readonly HttpClient httpClient;

    private readonly JsonSerializerOptions serializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public PayNowClient(HttpClient httpClient, IPayNowConfiguration configuration)
    {
        this.httpClient = httpClient;

        this.httpClient.BaseAddress = new Uri(configuration.ApiPath);
        this.httpClient.DefaultRequestHeaders.Add(PayNowConstants.HeadersNames.ApiKey, configuration.ApiKey);
        hmacSha256Calculator = new HmacSha256Calculator(encoding.GetBytes(configuration.SignatureKey));
    }

    public async Task<PostPaymentResponse> PostPaymentRequestAsync(PostPaymentRequest paymentRequest)
    {
        var builder = GetBuilder();
        builder.AddPaymentsPath();

        var response = await SendAndDeserializeAsync<PostPaymentResponse>(
            HttpMethod.Post,
            builder.ToString(),
            paymentRequest);

        if (response is null)
            throw new EmptyResponseException();

        return response;
    }

    public async Task<GetPaymentStatusResponse> GetPaymentStatusAsync(string paymentId)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentPath(paymentId)
            .AddPaymentStatusPath();

        var response = await SendAndDeserializeAsync<GetPaymentStatusResponse>(HttpMethod.Get, builder.ToString());

        if (response is null)
            throw new EmptyResponseException();

        return response;
    }

    public async Task<GetPaymentMethodsResponse> GetPaymentMethodsAsync(Currency currency = Currency.PLN)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentMethodsPath(currency);

        var response = await SendAndDeserializeAsync<GetPaymentMethodsResponse>(HttpMethod.Get, builder.ToString());

        if (response is null)
            throw new EmptyResponseException();

        return response;
    }

    public async Task<PostRefundResponse> PostRefundRequestAsync(string paymentId, PostRefundRequest refundRequest)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentPath(paymentId)
            .AddPaymentRefundsPath();

        var response = await SendAndDeserializeAsync<PostRefundResponse>(
            HttpMethod.Post,
            builder.ToString(),
            refundRequest
        );

        if (response is null)
            throw new EmptyResponseException();

        return response;
    }

    public async Task<GetRefundStatusResponse> GetRefundStatusAsync(string refundId)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentRefundsPath()
            .AddRefundPath(refundId)
            .AddRefundStatusPath();

        var response = await SendAndDeserializeAsync<GetRefundStatusResponse>(HttpMethod.Get, builder.ToString());

        if (response is null)
            throw new EmptyResponseException();

        return response;
    }

    private static PayNowApiPathBuilder GetBuilder() => new();

    private async Task<T?> SendAndDeserializeAsync<T>(HttpMethod method, string path, object? content = null)
    {
        var request = new HttpRequestMessage(method, path);

        if (content != null)
        {
            var serializedContent = JsonSerializer.Serialize(content, serializerOptions);
            request.Content = new StringContent(serializedContent, encoding, MediaTypeNames.Application.Json);

            request.Headers.Add(PayNowConstants.HeadersNames.Signature,
                hmacSha256Calculator.CalculateHmac(encoding.GetBytes(serializedContent)));
        }

        request.Headers.Add(PayNowConstants.HeadersNames.IdempotencyKey, Guid.NewGuid().ToString());

        using var response = await httpClient.SendAsync(request);
        {
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            {
                return await JsonSerializer.DeserializeAsync<T>(contentStream, serializerOptions);
            }
        }
    }
}
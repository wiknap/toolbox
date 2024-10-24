using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

using JetBrains.Annotations;

using Wiknap.PayNow.Exceptions;
using Wiknap.PayNow.Model;
using Wiknap.PayNow.Path;

namespace Wiknap.PayNow;

[PublicAPI]
public sealed class PayNowClient : IPayNowClient, IDisposable
{
    private readonly Encoding encoding = Encoding.UTF8;
    private readonly SignatureCalculator signatureCalculator;
    private readonly HttpClient httpClient;
    private readonly IPayNowConfiguration configuration;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public PayNowClient(HttpClient httpClient, IPayNowConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;

        this.httpClient.BaseAddress = new Uri(configuration.ApiPath);
        this.httpClient.DefaultRequestHeaders.Add(PayNowConstants.HeadersNames.ApiKey, configuration.ApiKey);
        this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        signatureCalculator =
            new SignatureCalculator(encoding.GetBytes(configuration.SignatureKey));
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

    public async Task<IReadOnlyCollection<AvailablePaymentMethod>> GetPaymentMethodsAsync(Currency currency = Currency.PLN,
        CancellationToken ct = default)
    {
        var builder = GetBuilder();
        builder
            .AddPaymentsPath()
            .AddPaymentMethodsPath(currency);

        var response =
            await SendAndDeserializeAsync<IReadOnlyCollection<AvailablePaymentMethod>>(HttpMethod.Get, builder.ToString(), ct: ct)
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

        AddHeaders(path, content, request);

        using var response = await httpClient.SendAsync(request, ct).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<T>(ct).ConfigureAwait(false);
    }

    private void AddHeaders(string path, object? content, HttpRequestMessage request)
    {
        var idempotencyKey = Guid.NewGuid().ToString();
        var queryParameters = HttpUtility.ParseQueryString(new Uri(new Uri(configuration.ApiPath), path).Query);
        var queryDictionary = queryParameters
            .Cast<string>()
            .ToDictionary(key => key, key => (string[]) [queryParameters[key] ?? string.Empty]);

        string? contentJson = null;

        if (content is not null)
        {
            var json = JsonSerializer.Serialize(content, jsonSerializerOptions);
            request.Content = new StringContent(json, encoding, "application/json");
            contentJson = json;
        }

        request.Headers.Add(PayNowConstants.HeadersNames.Signature,
            signatureCalculator.Calculate(configuration.ApiKey, idempotencyKey, queryDictionary, contentJson));
        request.Headers.Add(PayNowConstants.HeadersNames.IdempotencyKey, idempotencyKey);
    }

    public void Dispose() => signatureCalculator.Dispose();
}

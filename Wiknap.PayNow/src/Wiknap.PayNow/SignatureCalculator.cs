using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Wiknap.PayNow;

public sealed class SignatureCalculator : IDisposable
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly Encoding encoding = Encoding.UTF8;

    private readonly HMACSHA256 hmacSha256;

    public SignatureCalculator(byte[] keyBytes)
    {
        hmacSha256 = new HMACSHA256(keyBytes);
    }

    public string Calculate(string apiKey, string idempotencyKey, Dictionary<string, string[]> parameters, string? body = null)
    {
        var headers = new Dictionary<string, string>
        {
            { PayNowConstants.HeadersNames.ApiKey, apiKey },
            { PayNowConstants.HeadersNames.IdempotencyKey, idempotencyKey }
        };

        var payload = new
        {
            Headers = headers,
            Parameters = new SortedDictionary<string, string[]>(parameters),
            Body = body ?? string.Empty
        };
        var payloadJson = JsonSerializer.Serialize(payload, JsonSerializerOptions);
        return ConvertToBase64String(hmacSha256.ComputeHash(encoding.GetBytes(payloadJson)));
    }

    public bool IsNotificationSignatureValid(string signature, string content)
    {
        var contentBytes = encoding.GetBytes(content);
        return signature == ConvertToBase64String(hmacSha256.ComputeHash(contentBytes));
    }

    private static string ConvertToBase64String(byte[] byteArray) => Convert.ToBase64String(byteArray);

    public void Dispose() => hmacSha256.Dispose();
}

using Microsoft.Extensions.Options;

namespace Wiknap.PayNow.Configuration;

internal sealed class PayNowConfiguration : IPayNowConfiguration
{
    public PayNowConfiguration(IOptions<PayNowOptions> options)
    {
        ApiKey = options.Value.ApiKey;
        SignatureKey = options.Value.SignatureKey;
        ApiPath = options.Value.ApiPath;
    }

    public string ApiKey { get; }
    public string SignatureKey { get; }
    public string ApiPath { get; }
}
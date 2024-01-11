using JetBrains.Annotations;

using Microsoft.Extensions.Options;

namespace Wiknap.PayNow.Configuration;

[PublicAPI]
internal sealed class PayNowConfiguration : IPayNowConfiguration
{
    private readonly IOptions<PayNowOptions> options;

    public PayNowConfiguration(IOptions<PayNowOptions> options)
    {
        this.options = options;
    }

    public string ApiKey => options.Value.ApiKey;
    public string SignatureKey => options.Value.SignatureKey;
    public string ApiPath => options.Value.ApiPath;
}

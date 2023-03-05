namespace Wiknap.PayNow.Configuration;

internal sealed class PayNowOptions
{
    public const string SectionName = "PayNow";

    public string ApiKey { get; set; } = string.Empty;
    public string SignatureKey { get; set; } = string.Empty;
    public string ApiPath { get; set; } = string.Empty;
}
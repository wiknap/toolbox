using JetBrains.Annotations;

namespace Wiknap.PayNow.Configuration;

[UsedImplicitly]
internal sealed class PayNowOptions
{
    public const string SectionName = "PayNow";

    public string ApiKey { get; [UsedImplicitly] set; } = string.Empty;
    public string SignatureKey { get; [UsedImplicitly] set; } = string.Empty;
    public string ApiPath { get; [UsedImplicitly] set; } = string.Empty;
}
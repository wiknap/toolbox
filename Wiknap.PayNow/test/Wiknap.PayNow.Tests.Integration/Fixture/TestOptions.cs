namespace Wiknap.PayNow.Tests.Integration.Fixture;

public sealed class TestOptions : IPayNowConfiguration
{
    public required string ApiKey { get; set; }

    public required string SignatureKey { get; set; }

    public required string ApiPath { get; set; }
}

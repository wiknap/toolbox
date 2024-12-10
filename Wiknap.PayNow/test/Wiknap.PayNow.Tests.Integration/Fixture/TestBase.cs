using Xunit;

namespace Wiknap.PayNow.Tests.Integration.Fixture;

[Collection("Configuration")]
public abstract class TestBase(TestConfiguration configuration) : IDisposable
{
    protected readonly PayNowClient PayNowClient = new(new HttpClient(), configuration.StandardOptions);
    protected readonly PayNowClient WhiteLabelPayNowClient = new(new HttpClient(), configuration.WhiteLabelOptions);
    private readonly CancellationTokenSource cts = new();

    protected CancellationToken CancellationToken => cts.Token;

    public void Dispose()
    {
        WhiteLabelPayNowClient.Dispose();
        PayNowClient.Dispose();
        cts.Dispose();
    }
}

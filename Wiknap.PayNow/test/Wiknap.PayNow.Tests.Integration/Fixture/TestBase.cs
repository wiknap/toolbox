using Xunit;

namespace Wiknap.PayNow.Tests.Integration.Fixture;

[Collection("Configuration")]
public abstract class TestBase(TestConfiguration configuration) : IDisposable
{
    protected readonly PayNowClient PayNowClient = new(new HttpClient(), configuration.StandardOptions);
    protected readonly PayNowClient WhiteLabelPayNowClient = new(new HttpClient(), configuration.WhiteLabelOptions);
    protected readonly CancellationTokenSource Cts = new();

    public void Dispose()
    {
        WhiteLabelPayNowClient.Dispose();
        PayNowClient.Dispose();
        Cts.Dispose();
    }
}

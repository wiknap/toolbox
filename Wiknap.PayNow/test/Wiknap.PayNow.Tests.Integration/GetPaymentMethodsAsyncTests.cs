using Shouldly;

using Wiknap.PayNow.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.PayNow.Tests.Integration;

public sealed class GetPaymentMethodsAsyncTests(TestConfiguration configuration) : TestBase(configuration)
{
    [Fact]
    public async Task When_GetPaymentMethodsAsync_Then_ReturnsPaymentMethods()
    {
        // Arrange & Act
        var methods = await PayNowClient.GetPaymentMethodsAsync(ct: CancellationToken);

        // Assert
        methods.ShouldNotBeNull();
        methods.ShouldNotBeEmpty();
    }
}

using Shouldly;

using Wiknap.PayNow.Model;
using Wiknap.PayNow.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.PayNow.Tests.Integration;

public sealed class PostPaymentAsyncTests(TestConfiguration configuration) : TestBase(configuration)
{
    [Fact]
    public async Task Given_ValidRequest_When_PostPaymentRequestAsync_When_ReturnsValidResponse()
    {
        // Arrange
        var request = new PostPaymentRequest(100M, "Id", "Description", new Buyer { Email = "test@test.com" });

        // Act
        var response = await PayNowClient.PostPaymentRequestAsync(request, Cts.Token);

        // Assert
        response.ShouldNotBeNull();
        response.Status.ShouldBe(Status.New);
        response.PaymentId.ShouldNotBeEmpty();
        response.RedirectUrl.ShouldNotBeEmpty();
    }
}

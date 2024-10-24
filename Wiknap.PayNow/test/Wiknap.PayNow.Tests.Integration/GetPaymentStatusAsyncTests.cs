using Shouldly;

using Wiknap.PayNow.Model;
using Wiknap.PayNow.Tests.Integration.Fixture;

using Xunit;

namespace Wiknap.PayNow.Tests.Integration;

public sealed class GetPaymentStatusAsyncTests(TestConfiguration configuration) : TestBase(configuration)
{
    [Fact]
    public async Task Given_ExistingPaymentRequest_When_GetPaymentStatusAsync_Then_ReturnsValidResponse()
    {
        // Arrange
        var request = new PostPaymentRequest(100M, "Id", "Description", new Buyer { Email = "test@test.com" });
        var postPaymentResponse = await PayNowClient.PostPaymentRequestAsync(request, Cts.Token);

        // Act
        var response = await PayNowClient.GetPaymentStatusAsync(postPaymentResponse.PaymentId);

        // Assert
        response.ShouldNotBeNull();
        response.PaymentId.ShouldBe(postPaymentResponse.PaymentId);
        response.Status.ShouldBe(PaymentStatus.New);
    }
}

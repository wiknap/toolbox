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

    [Fact]
    public async Task Given_WhiteLabelValidRequest_When_PostPaymentRequestAsync_When_ReturnsValidResponse()
    {
        // Arrange
        var request = new PostPaymentRequest(1M, "Id", "Description",
            new Buyer
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Phone = new Phone { Prefix = "+48", Number = "666666666" },
                Email = "test@test.com",
            },
            paymentMethodId: 2007,
            authorizationCode: "111111");

        // Act
        var response = await WhiteLabelPayNowClient.PostPaymentRequestAsync(request, Cts.Token);

        // Assert
        response.ShouldNotBeNull();
        response.Status.ShouldBe(Status.Pending);
        response.PaymentId.ShouldNotBeEmpty();
        response.RedirectUrl.ShouldBeNull();
    }
}

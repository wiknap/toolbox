using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class GetPaymentStatusResponseTests : TestsBase
{
    [Fact]
    public void Given_ValidRequest_When_Serialize_Then_ReturnsCorrectJson()
    {
        // Arrange
        var method = new GetPaymentStatusResponse
        {
            PaymentId = Faker.Lorem.Word(),
            Status = PaymentStatus.New
        };

        // Act & Assert
        var serialized = Should.NotThrow(() => JsonSerializer.Serialize(method));

        serialized.ShouldBe("{" +
                                $"\"paymentId\":\"{method.PaymentId}\"," +
                                "\"status\":\"NEW\"" +
                            "}");
    }

    [Fact]
    public void Given_ValidJson_When_Deserialize_Then_ReturnObject()
    {
        // Arrange
        var id = Faker.Lorem.Word();
        var json = "{" +
                        $"\"paymentId\":\"{id}\"," +
                        "\"status\":\"NEW\"" +
                    "}";

        // Act & Assert
        var deserialized = JsonSerializer.Deserialize<GetPaymentStatusResponse>(json);

        deserialized.ShouldNotBeNull();
        deserialized.PaymentId.ShouldBe(id);
        deserialized.Status.ShouldBe(PaymentStatus.New);
    }
}

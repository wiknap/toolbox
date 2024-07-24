using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class PostPaymentResponseTests : TestsBase
{
    [Fact]
    public void Given_ValidRequest_When_Serialize_Then_ReturnsCorrectJson()
    {
        // Arrange
        var method = new GetRefundStatusResponse
        {
            RefundId = Faker.Lorem.Word(),
            Status = GetRefundStatus.New,
            FailureReason = GetFailureReason.Other
        };

        // Act & Assert
        var serialized = Should.NotThrow(() => JsonSerializer.Serialize(method));

        serialized.ShouldBe("{" +
                                $"\"refundId\":\"{method.RefundId}\"," +
                                "\"status\":\"NEW\"," +
                                "\"failureReason\":\"OTHER\"" +
                            "}");
    }

    [Fact]
    public void Given_ValidJson_When_Deserialize_Then_ReturnObject()
    {
        // Arrange
        var id = Faker.Lorem.Word();
        var json = "{" +
                        $"\"refundId\":\"{id}\"," +
                        "\"status\":\"NEW\"," +
                        "\"failureReason\":\"OTHER\"" +
                    "}";

        // Act & Assert
        var deserialized = JsonSerializer.Deserialize<GetRefundStatusResponse>(json);

        deserialized.ShouldNotBeNull();
        deserialized.RefundId.ShouldBe(id);
        deserialized.Status.ShouldBe(GetRefundStatus.New);
        deserialized.FailureReason.ShouldBe(GetFailureReason.Other);
    }
}

using System.Text.Json;

using Bogus;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class PostRefundRequestTests : TestsBase
{
    [Fact]
    public void Given_ValidRequest_When_Serialize_Then_ReturnsCorrectJson()
    {
        // Arrange
        var request = new PostRefundRequest(Faker.Random.Decimal(min: 1, max: 1000M), RefundReason.RefundAfter14);

        // Act & Assert
        var deserialized = JsonSerializer.Serialize(request);

        deserialized.ShouldBe(
            $"{{" +
                $"\"amount\":{(int)Math.Floor(request.Amount * 100)}," +
                $"\"reason\":\"REFUND_AFTER_14\"" +
            $"}}");
    }

    [Fact]
    public void Given_ValidJson_When_Deserialize_Then_ReturnsObject()
    {
        // Arrange
        var amount = Faker.Random.Int(min: 1, max: 100000);
        var json =
            $"{{" +
                $"\"amount\":{amount}," +
                $"\"reason\":\"REFUND_AFTER_14\"" +
            $"}}";

        // Act & Assert
        var deserialized = JsonSerializer.Deserialize<PostRefundRequest>(json);

        deserialized.ShouldNotBeNull();
        deserialized.AmountAsInt.ShouldBe(amount);
        deserialized.Amount.ShouldBe(decimal.Divide(amount, 100));
        deserialized.Reason.ShouldBe(RefundReason.RefundAfter14);
    }
}

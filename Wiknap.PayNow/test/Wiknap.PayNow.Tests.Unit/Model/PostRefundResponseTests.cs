using System.Text.Json;

using Bogus;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class PostRefundResponseTests : TestsBase
{
    [Fact]
    public void Given_ValidRequest_When_Serialize_Then_ReturnsCorrectJson()
    {
        // Arrange
        var request = new PostRefundResponse
        {
            RefundId = Faker.Lorem.Word(),
            Status = PostRefundStatus.New
        };

        // Act & Assert
        var deserialized = JsonSerializer.Serialize(request);

        deserialized.ShouldBe(
            $"{{" +
                $"\"refundId\":\"{request.RefundId}\"," +
                $"\"status\":\"NEW\"" +
            $"}}");
    }

    [Fact]
    public void Given_ValidJson_When_Deserialize_Then_ReturnsObject()
    {
        // Arrange
        var id = Faker.Lorem.Word();
        var json =
            $"{{" +
                $"\"refundId\":\"{id}\"," +
                $"\"status\":\"NEW\"" +
            $"}}";

        // Act & Assert
        var deserialized = JsonSerializer.Deserialize<PostRefundResponse>(json);

        deserialized.ShouldNotBeNull();
        deserialized.RefundId.ShouldBe(id);
        deserialized.Status.ShouldBe(PostRefundStatus.New);
    }
}

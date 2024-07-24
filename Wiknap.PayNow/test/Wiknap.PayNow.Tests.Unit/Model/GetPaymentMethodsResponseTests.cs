using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class GetPaymentMethodsResponseTests : TestsBase
{
    [Fact]
    public void Given_ValidRequest_When_Serialize_Then_ReturnsCorrectJson()
    {
        // Arrange
        var method = new PaymentMethod
        {
            Id = Faker.Random.Int(0),
            AuthorizationType = AuthorizationType.Code,
            ImageUrl = Faker.Internet.Url(),
            Name = Faker.Lorem.Word(),
            Status = PaymentMethodStatus.Enabled
        };
        var response = new GetPaymentMethodsResponse
        {
            Type = PaymentMethodType.Blik,
            PaymentMethods = [method]
        };

        // Act & Assert
        var serialized = Should.NotThrow(() => JsonSerializer.Serialize(response));

        serialized.ShouldBe("{" +
                                "\"type\":\"BLIK\"," +
                                "\"paymentMethods\":[" +
                                    "{" +
                                        $"\"id\":{method.Id}," +
                                        $"\"name\":\"{method.Name}\"," +
                                        "\"description\":null," +
                                        $"\"image\":\"{method.ImageUrl}\"," +
                                        "\"status\":\"ENABLED\"," +
                                        "\"authorizationType\":\"CODE\"" +
                                    "}" +
                                "]" +
                            "}");
    }

    [Fact]
    public void Given_ValidJson_When_Deserialize_Then_ReturnObject()
    {
        // Arrange
        var id = Faker.Random.Int(0);
        var name = Faker.Lorem.Word();
        var url = Faker.Internet.Url();
        var json = "{" +
                        "\"type\":\"BLIK\"," +
                        "\"paymentMethods\":[" +
                            "{" +
                                $"\"id\":{id}," +
                                $"\"name\":\"{name}\"," +
                                "\"description\":null," +
                                $"\"image\":\"{url}\"," +
                                "\"status\":\"ENABLED\"," +
                                "\"authorizationType\":\"CODE\"" +
                            "}" +
                        "]" +
                    "}";

        // Act & Assert
        var deserialized = JsonSerializer.Deserialize<GetPaymentMethodsResponse>(json);

        deserialized.ShouldNotBeNull();
        deserialized.Type.ShouldBe(PaymentMethodType.Blik);
        deserialized.PaymentMethods.Length.ShouldBe(1);
        var method = deserialized.PaymentMethods.First();
        method.Id.ShouldBe(id);
        method.Name.ShouldBe(name);
        method.Description.ShouldBeNull();
        method.ImageUrl.ShouldBe(url);
        method.Status.ShouldBe(PaymentMethodStatus.Enabled);
        method.AuthorizationType.ShouldBe(AuthorizationType.Code);
    }
}

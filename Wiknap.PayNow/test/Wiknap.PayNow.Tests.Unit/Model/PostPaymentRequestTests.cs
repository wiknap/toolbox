using System.Text.Json;

using Bogus;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit.Model;

public sealed class PostPaymentRequestTests : TestsBase
{
    [Fact]
    public void Given_ValidRequest_When_Serialize_Then_ReturnsCorrectJson()
    {
        // Arrange
        var request = new PostPaymentRequest(Faker.Random.Decimal(min: 1, max: 1000M), Faker.Lorem.Word(),
            Faker.Lorem.Sentence(), new Buyer { Email = Faker.Internet.Email() });

        // Act & Assert
        var serialized = Should.NotThrow(() => JsonSerializer.Serialize(request));
        serialized.ShouldBe(
            $"{{" +
                $"\"amount\":{(int)Math.Floor(request.Amount * 100)}," +
                $"\"currency\":null," +
                $"\"externalId\":\"{request.ExternalId}\"," +
                $"\"description\":\"{request.Description}\"," +
                $"\"continueUrl\":null," +
                $"\"buyer\":{{" +
                    $"\"email\":\"{request.Buyer.Email}\"," +
                    $"\"firstName\":null," +
                    $"\"lastName\":null," +
                    $"\"phone\":null," +
                    $"\"locale\":null" +
                $"}}," +
                $"\"paymentMethodId\":null," +
                $"\"authorizationCode\":null" +
            $"}}");
    }

    [Fact]
    public void Given_ValidJson_When_Deserialize_Then_ReturnObject()
    {
        //Arrange
        var amount = Faker.Random.Int(min: 1, max: 100000);
        var id = Faker.Lorem.Word();
        var desc = Faker.Lorem.Sentence();
        var email = Faker.Internet.Email();
        var json =
            $"{{" +
                $"\"amount\":{amount}," +
                $"\"currency\":null," +
                $"\"externalId\":\"{id}\"," +
                $"\"description\":\"{desc}\"," +
                $"\"continueUrl\":null," +
                $"\"buyer\":{{" +
                    $"\"email\":\"{email}\"," +
                    $"\"firstName\":null," +
                    $"\"lastName\":null," +
                    $"\"phone\":null," +
                    $"\"locale\":null" +
                $"}}" +
            $"}}";

        // Act & Assert
        var deserialized = Should.NotThrow(() => JsonSerializer.Deserialize<PostPaymentRequest>(json));

        deserialized.ShouldNotBeNull();
        deserialized.AmountAsInt.ShouldBe(amount);
        deserialized.Amount.ShouldBe(decimal.Divide(amount, 100));
        deserialized.Currency.ShouldBeNull();
        deserialized.ExternalId.ShouldBe(id);
        deserialized.Description.ShouldBe(desc);
        deserialized.ContinueUrl.ShouldBeNull();
        deserialized.Buyer.Email.ShouldBe(email);
        deserialized.Buyer.FirstName.ShouldBeNull();
        deserialized.Buyer.LastName.ShouldBeNull();
        deserialized.Buyer.Phone.ShouldBeNull();
        deserialized.Buyer.Locale.ShouldBeNull();
    }
}

using System.Text.Json;

using Shouldly;

using Wiknap.PayNow.Model;

using Xunit;

namespace Wiknap.PayNow.Tests.Unit;

public sealed class SignatureCalculatorTests
{
    private readonly SignatureCalculator calculator = new("b305b996-bca5-4404-a0b7-2ccea3d2b64b"u8.ToArray());
    private const string ApiKey = "97a55694-5478-43b5-b406-fb49ebfdd2b5";

    [Fact]
    public void Given_OnlyIdempotencyKey_When_Calculate_Then_Returns_ValidSignature()
    {
        // Arrange & Act
        var result = calculator.Calculate(ApiKey, "d243fdb3-c287-484a-bb9c-58536f2794c1", []);

        // Assert
        result.ShouldBe("fXwLZRwo0WiGll90PPl5oULX9VKA0gpFA/3+E+NRp5E=");
    }

    [Fact]
    public void Given_IdempotencyKeyAndParameters_When_Calculate_Then_Returns_ValidSignature()
    {
        // Arrange & Act
        var result = calculator.Calculate(ApiKey, "4b61cfe1-9d61-4973-85af-513d63e87eea",
            new Dictionary<string, string[]> { { "currency", ["PLN"] } });

        // Assert
        result.ShouldBe("dJvJGEf9ERzrXkDGyNxGH/Bzzv0RVRX+1jZ5EPv7sWk=");
    }

    [Fact]
    public void Given_IdempotencyKeyAndBody_When_Calculate_Then_Returns_ValidSignature()
    {
        // Arrange
        var request = new PostPaymentRequest(100M, "1234", "1234", new Buyer { Email = "test@test.com" });

        // Act
        var result =
            calculator.Calculate(ApiKey, "b63dc872-9034-41ca-af9d-86ddf6b41d5c", [], JsonSerializer.Serialize(request));

        // Assert
        result.ShouldBe("cv1ta5ltOk9294cEzCZ3f626RzBIl3e53r56GRSJ2q4=");
    }

    [Fact]
    public void Given_ContentAndMatchingSignature_When_IsNotificationSignatureValid_Then_Returns_True()
    {
        // Arrange
        const string notification =
            "{\"paymentId\":\"NOLV-8F9-08K-WGD\",\"externalId\":\"12345\",\"status\":\"CONFIRMED\",\"modifiedAt\":\"2018-12-12T13:24:52\"}";

        // Act
        var result =
            calculator.IsNotificationSignatureValid("xtiaCua1Y+uBkPA2hl48m6I5kqn6bHVa9KpNvtMyMcQ=", notification);

        // Assert
        result.ShouldBeTrue();
    }
}

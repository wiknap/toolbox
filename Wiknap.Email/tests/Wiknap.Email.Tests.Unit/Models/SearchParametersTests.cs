using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class SearchParametersTests : TestsBase
{
    [Fact]
    public void Given_ValidInput_When_Constructor_Then_ValidSearchParametersCreated()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var email = Faker.Internet.Email();
        var date = Faker.Date.Past();

        // Act
        var parameters = new SearchParameters { Subject = subject, SenderEmail = email, DeliveredAfter = date };

        // Assert
        parameters.Subject.ShouldBe(subject);
        parameters.SenderEmail.ShouldBe(email);
        parameters.DeliveredAfter.ShouldBe(date);
    }
}

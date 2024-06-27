using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class EmailBodyTests : TestsBase
{
    [Fact]
    public void Given_ValidMessage_When_Constructor_Then_ValidEmailBodyWithTypeTextCreated()
    {
        // Arrange
        var message = Faker.Lorem.Sentence();

        // Act
        var body = new EmailBody(message);

        // Assert
        body.Message.ShouldBe(message);
        body.ContentType.ShouldBe(EmailContentType.Text);
    }

    [Fact]
    public void Given_ValidMessageAndTypeHtml_When_Constructor_Then_ValidEmailBodyWithTypeHtmlCreated()
    {
        // Arrange
        var message = Faker.Lorem.Sentence();

        // Act
        var body = new EmailBody(message, EmailContentType.Html);

        // Assert
        body.Message.ShouldBe(message);
        body.ContentType.ShouldBe(EmailContentType.Html);
    }

    [Fact]
    public void Given_ValidMessage_When_ImplicitOperator_Then_ValidEmailBodyWithTypeTextCreated()
    {
        // Arrange
        var message = Faker.Lorem.Sentence();

        // Act
        EmailBody body = message;

        // Assert
        body.Message.ShouldBe(message);
        body.ContentType.ShouldBe(EmailContentType.Text);
    }
}

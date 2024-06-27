using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.Models;

public sealed class EmailMessageTests : TestsBase
{
    [Fact]
    public void Given_ValidSubjectAndMessage_When_Constructor_Then_ValidEmailMessageCreated()
    {
        // Arrange
        var subject = Faker.Lorem.Word();
        var message = Faker.Lorem.Sentence();

        // Act
        var emailMessage = new EmailMessage { Subject = subject, Body = message };

        // Assert
        emailMessage.Subject.ShouldBe(subject);
        emailMessage.Body.ShouldNotBeNull();
        emailMessage.Body.Message.ShouldBe(message);
        emailMessage.Body.ContentType.ShouldBe(EmailContentType.Text);
        emailMessage.Recipients.ShouldBeEmpty();
        emailMessage.Attachments.ShouldBeEmpty();
    }
}

using NSubstitute;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit;

public sealed class EmailClientExtensionsTests : TestsBase
{
    private readonly IEmailClient client = Substitute.For<IEmailClient>();

    [Fact]
    public async Task Given_ValidEmailAndSubjectAndTextMessage_When_SendEmailAsync_Then_ValidEmailMessageWithTextBodySend()
    {
        // Arrange
        var email = Faker.Internet.Email();
        var subject = Faker.Lorem.Word();
        var message = Faker.Lorem.Sentence();

        // Act
        await client.SendEmailAsync(email, subject, message);

        // Assert
        await client.Received().SendEmailAsync(
            Arg.Is<EmailMessage>(m =>
                m.Subject == subject &&
                m.Recipients.Count == 1 &&
                m.Recipients.First().EmailAddress.Email == email &&
                m.Recipients.First().EmailAddress.Name == null &&
                m.Recipients.First().Type == RecipientType.To &&
                m.Body != null &&
                m.Body.Message == message &&
                m.Body.ContentType == EmailContentType.Text),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_ValidEmailAndSubjectAndHtmlMessage_When_SendEmailAsync_Then_ValidEmailMessageWithHtmlBodySend()
    {
        // Arrange
        var email = Faker.Internet.Email();
        var subject = Faker.Lorem.Word();
        var message = Faker.Lorem.Sentence();

        // Act
        await client.SendEmailAsync(email, subject, message, true);

        // Assert
        await client.Received().SendEmailAsync(
            Arg.Is<EmailMessage>(m =>
                m.Subject == subject &&
                m.Recipients.Count == 1 &&
                m.Recipients.First().EmailAddress.Email == email &&
                m.Recipients.First().EmailAddress.Name == null &&
                m.Recipients.First().Type == RecipientType.To &&
                m.Body != null &&
                m.Body.Message == message &&
                m.Body.ContentType == EmailContentType.Html),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_ValidRecipientAndSubjectAndTextMessage_When_SendEmailAsync_Then_ValidEmailMessageWithTextBodySend()
    {
        // Arrange
        var recipient = new Recipient(new EmailAddress(Faker.Internet.Email(), Faker.Lorem.Sentence()));
        var subject = Faker.Lorem.Word();
        var message = Faker.Lorem.Sentence();

        // Act
        await client.SendEmailAsync([recipient], subject, message);

        // Assert
        await client.Received().SendEmailAsync(
            Arg.Is<EmailMessage>(m =>
                m.Subject == subject &&
                m.Recipients.Count == 1 &&
                m.Recipients.First().EmailAddress.Email == recipient.EmailAddress.Email &&
                m.Recipients.First().EmailAddress.Name == recipient.EmailAddress.Name &&
                m.Recipients.First().Type == RecipientType.To &&
                m.Body != null &&
                m.Body.Message == message &&
                m.Body.ContentType == EmailContentType.Text),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Given_ValidRecipientAndSubjectAndHtmlMessage_When_SendEmailAsync_Then_ValidEmailMessageWithTextBodySend()
    {
        // Arrange
        var recipient = new Recipient(new EmailAddress(Faker.Internet.Email(), Faker.Lorem.Sentence()));
        var subject = Faker.Lorem.Word();
        var message = Faker.Lorem.Sentence();

        // Act
        await client.SendEmailAsync([recipient], subject, message, true);

        // Assert
        await client.Received().SendEmailAsync(
            Arg.Is<EmailMessage>(m =>
                m.Subject == subject &&
                m.Recipients.Count == 1 &&
                m.Recipients.First().EmailAddress.Email == recipient.EmailAddress.Email &&
                m.Recipients.First().EmailAddress.Name == recipient.EmailAddress.Name &&
                m.Recipients.First().Type == RecipientType.To &&
                m.Body != null &&
                m.Body.Message == message &&
                m.Body.ContentType == EmailContentType.Html),
            Arg.Any<CancellationToken>());
    }
}

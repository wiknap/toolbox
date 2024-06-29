using MimeKit;

using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.EmailMessageExtensions;

public sealed class ToMimeMessageTests : TestsBase
{
    [Fact]
    public void Given_ValidSenderEmail_When_ToMimeMessage_Then_MimeMessageWithNoSubjectAndBodyAndAttachmentsCreated()
    {
        // Arrange
        var sender = Faker.Internet.Email();
        var emailMessage = new EmailMessage();

        // Act
        var mimeMessage = emailMessage.ToMimeMessage(new MailboxAddress(null, sender));

        // Assert
        mimeMessage.From.Count.ShouldBe(1);
        var from = mimeMessage.From.Mailboxes.First();
        from.Name.ShouldBeNull();
        from.Address.ShouldBe(sender);
        mimeMessage.To.ShouldBeEmpty();
        mimeMessage.Cc.ShouldBeEmpty();
        mimeMessage.Bcc.ShouldBeEmpty();
        mimeMessage.Subject.ShouldBeEmpty();
        mimeMessage.Body.ShouldBeNull();
        mimeMessage.Attachments.ShouldBeEmpty();
    }

    [Fact]
    public void Given_ValidSenderEmailAndBodyAndToRecipientAndSubjectAndAttachment_When_ToMimeMessage_Then_MimeMessageWithSubjectAndBodyAndOneAttachmentCreated()
    {
        // Arrange
        var sender = Faker.Internet.Email();
        var toEmail = Faker.Internet.Email();
        var message = Faker.Lorem.Sentence();
        var subject = Faker.Lorem.Word();
        const string fileName = "file.txt";
        var emailMessage = new EmailMessage
        {
            Subject = subject,
            Body = message
        };

        emailMessage.Recipients.Add(toEmail);
        emailMessage.Attachments.Add(new EmailAttachment(fileName, EmailAttachmentType.Png, Stream.Null));

        // Act
        var mimeMessage = emailMessage.ToMimeMessage(new MailboxAddress(null, sender));

        // Assert
        mimeMessage.From.Count.ShouldBe(1);
        var from = mimeMessage.From.Mailboxes.First();
        from.Name.ShouldBeNull();
        from.Address.ShouldBe(sender);
        mimeMessage.To.Count.ShouldBe(1);
        var to = mimeMessage.To.Mailboxes.First();
        to.Name.ShouldBeNull();
        to.Address.ShouldBe(toEmail);
        mimeMessage.Cc.ShouldBeEmpty();
        mimeMessage.Bcc.ShouldBeEmpty();
        mimeMessage.Subject.ShouldBe(subject);
        mimeMessage.Body.ShouldNotBeNull();
        var multipart = mimeMessage.Body.ShouldBeOfType<Multipart>();
        multipart.Count.ShouldBe(2);
        var textPart = multipart.OfType<TextPart>().SingleOrDefault();
        textPart.ShouldNotBeNull();
        textPart.ContentType.MimeType.ShouldBe("text/plain");
        textPart.Text.ShouldBe(message);
        var mimePart = multipart.OfType<MimePart>().SingleOrDefault(p => p.ContentType.MimeType == "image/png");
        mimePart.ShouldNotBeNull();
        mimePart.FileName.ShouldBe(fileName);
        mimePart.Content.Stream.ShouldNotBeNull();
        mimeMessage.Attachments.Count().ShouldBe(1);
    }

    [Fact]
    public void Given_ValidSenderEmailAndWithoutBodyAndToRecipientAndSubjectAndAttachment_When_ToMimeMessage_Then_MimeMessageWithSubjectAndBodyAndOneAttachmentCreated()
    {
        // Arrange
        var sender = Faker.Internet.Email();
        var toEmail = Faker.Internet.Email();
        var subject = Faker.Lorem.Word();
        const string fileName = "file.txt";
        var emailMessage = new EmailMessage
        {
            Subject = subject
        };

        emailMessage.Recipients.Add(toEmail);
        emailMessage.Attachments.Add(new EmailAttachment(fileName, EmailAttachmentType.Png, Stream.Null));

        // Act
        var mimeMessage = emailMessage.ToMimeMessage(new MailboxAddress(null, sender));

        // Assert
        mimeMessage.From.Count.ShouldBe(1);
        var from = mimeMessage.From.Mailboxes.First();
        from.Name.ShouldBeNull();
        from.Address.ShouldBe(sender);
        mimeMessage.To.Count.ShouldBe(1);
        var to = mimeMessage.To.Mailboxes.First();
        to.Name.ShouldBeNull();
        to.Address.ShouldBe(toEmail);
        mimeMessage.Cc.ShouldBeEmpty();
        mimeMessage.Bcc.ShouldBeEmpty();
        mimeMessage.Subject.ShouldBe(subject);
        mimeMessage.Body.ShouldNotBeNull();
        var multipart = mimeMessage.Body.ShouldBeOfType<Multipart>();
        multipart.Count.ShouldBe(1);
        multipart.OfType<TextPart>().ShouldBeEmpty();
        var mimePart = multipart.OfType<MimePart>().SingleOrDefault(p => p.ContentType.MimeType == "image/png");
        mimePart.ShouldNotBeNull();
        mimePart.FileName.ShouldBe(fileName);
        mimePart.Content.Stream.ShouldNotBeNull();
        mimeMessage.Attachments.Count().ShouldBe(1);
    }
}

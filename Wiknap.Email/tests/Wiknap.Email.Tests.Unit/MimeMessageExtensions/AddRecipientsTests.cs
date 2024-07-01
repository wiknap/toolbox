using MimeKit;

using Shouldly;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Unit.MimeMessageExtensions;

public sealed class AddRecipientsTests : TestsBase
{
    [Fact]
    public void Given_TextBody_When_ToTextPart_Then_TextPartWithMessageAndTextMimeTypeCreated()
    {
        // Arrange
        var message = new MimeMessage();
        var toEmail = Faker.Internet.Email();
        var ccEmail = Faker.Internet.Email();
        var bccEmail = Faker.Internet.Email();
        var to = new Recipient(toEmail);
        var cc = new Recipient(ccEmail, RecipientType.Cc);
        var bcc = new Recipient(bccEmail, RecipientType.Bcc);

        // Act
        message.AddRecipients([to, cc, bcc]);

        // Assert
        message.To.Mailboxes.Count().ShouldBe(1);
        var toMailbox = message.To.Mailboxes.First();
        toMailbox.Address.ShouldBe(toEmail);
        message.Cc.Mailboxes.Count().ShouldBe(1);
        var ccMailbox = message.Cc.Mailboxes.First();
        ccMailbox.Address.ShouldBe(ccEmail);
        message.Bcc.Mailboxes.Count().ShouldBe(1);
        var bccMailbox = message.Bcc.Mailboxes.First();
        bccMailbox.Address.ShouldBe(bccEmail);
    }
}

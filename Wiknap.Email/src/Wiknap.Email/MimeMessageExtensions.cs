using MimeKit;

using Wiknap.Email.Models;

namespace Wiknap.Email;

public static class MimeMessageExtensions
{
    public static void AddRecipients(this MimeMessage message, IReadOnlyCollection<Recipient> recipients)
    {
        foreach (var recipient in recipients)
        {
            switch (recipient.Type)
            {
                case RecipientType.Cc:
                    message.Cc.Add(new MailboxAddress(recipient.EmailAddress.Name, recipient.EmailAddress.Email));
                    break;
                case RecipientType.Bcc:
                    message.Bcc.Add(new MailboxAddress(recipient.EmailAddress.Name, recipient.EmailAddress.Email));
                    break;
                case RecipientType.To:
                default:
                    message.To.Add(new MailboxAddress(recipient.EmailAddress.Name, recipient.EmailAddress.Email));
                    break;
            }
        }
    }

    public static EmailContent GetEmailContent(this MimeMessage message)
    {
        var body = !string.IsNullOrEmpty(message.HtmlBody)
            ? new EmailBody(message.HtmlBody, EmailContentType.Html)
            : !string.IsNullOrEmpty(message.TextBody)
                ? new EmailBody(message.TextBody)
                : null;

        var attachments = new HashSet<EmailAttachment>();
        foreach (var attachment in message.Attachments)
        {
            var emailAttachment = attachment.ToEmailAttachment();

            if (emailAttachment is null)
                continue;

            attachments.Add(emailAttachment);
        }

        return new EmailContent(body, [.. attachments]);
    }

    public static EmailAttachment? ToEmailAttachment(this MimeEntity attachment)
    {
        var memoryStream = new MemoryStream();
        switch (attachment)
        {
            case MessagePart rfc822:
                rfc822.Message.WriteTo(memoryStream);
                break;
            case MimePart part:
                part.Content.DecodeTo(memoryStream);
                break;
            default:
                return null;
        }

        var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
        return new EmailAttachment(fileName, attachment.ContentType.GetAttachmentType(), memoryStream);
    }

    public static EmailAttachmentType GetAttachmentType(this ContentType contentType)
    {
        return contentType.MimeType switch
        {
            MimeTypes.Images.Png => EmailAttachmentType.Png,
            MimeTypes.Images.Gif => EmailAttachmentType.Gif,
            MimeTypes.Message.Rfc822 => EmailAttachmentType.Rfc822,
            _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null)
        };
    }
}

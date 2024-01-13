using MimeKit;

namespace Wiknap.Email;

public static class MimeMessageExtensions
{
    public static void AddRecipients(this MimeMessage message, Recipient[] recipients)
    {
        foreach (var recipient in recipients)
        {
            switch (recipient.Type)
            {
                case RecipientType.Cc:
                    message.Cc.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    break;
                case RecipientType.Bcc:
                    message.Bcc.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    break;
                case RecipientType.To:
                default:
                    message.To.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    break;
            }
        }
    }

    public static string GetMessageContent(this MimeMessage message, EmailContentType? contentType)
    {
        return contentType switch
        {
            EmailContentType.Html => message.HtmlBody,
            EmailContentType.Text => message.TextBody,
            _ => !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody
        };
    }
}

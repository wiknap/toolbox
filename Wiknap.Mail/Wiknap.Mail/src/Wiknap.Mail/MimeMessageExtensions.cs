using MimeKit;

namespace Wiknap.Mail;

public static class MimeMessageExtensions
{
    public static string GetMessageContent(this MimeMessage message, MailContentType? contentType)
    {
        return contentType switch
        {
            MailContentType.Html => message.HtmlBody,
            MailContentType.Text => message.TextBody,
            _ => !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody
        };
    }
}
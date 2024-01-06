using MimeKit;

namespace Wiknap.Email;

public static class MimeMessageExtensions
{
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
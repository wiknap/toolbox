using MimeKit;

using Wiknap.Email.Models;

namespace Wiknap.Email;

public static class EmailMessageExtensions
{
    public static MimeMessage ToMimeMessage(this EmailMessage emailMessage, MailboxAddress fromAddress)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(fromAddress);
        mimeMessage.AddRecipients(emailMessage.Recipients);

        if (!string.IsNullOrEmpty(emailMessage.Subject))
            mimeMessage.Subject = emailMessage.Subject;

        mimeMessage.Body = GetMimeMessageBody(emailMessage);

        return mimeMessage;
    }

    private static MimeEntity? GetMimeMessageBody(EmailMessage emailMessage)
    {
        var text = emailMessage.Body?.ToTextPart();

        if (emailMessage.Attachments.Count == 0)
            return text;

        var multipart = new Multipart("mixed");
        if (text is not null)
            multipart.Add(text);

        foreach (var attachment in emailMessage.Attachments)
        {
            var attachmentPart = new MimePart(attachment.Type.ToContentType())
            {
                Content = new MimeContent(attachment.Content),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachment.Filename
            };

            multipart.Add(attachmentPart);
        }

        return multipart.Count > 0 ? multipart : null;
    }

    public static ContentType ToContentType(this EmailAttachmentType attachmentType)
    {
        return attachmentType switch
        {
            EmailAttachmentType.Gif => ContentType.Parse(MimeTypes.Images.Gif),
            EmailAttachmentType.Png => ContentType.Parse(MimeTypes.Images.Png),
            EmailAttachmentType.Jpeg => ContentType.Parse(MimeTypes.Images.Jpeg),
            EmailAttachmentType.Svg => ContentType.Parse(MimeTypes.Images.Svg),
            EmailAttachmentType.Bmp => ContentType.Parse(MimeTypes.Images.Bmp),
            EmailAttachmentType.Csv => ContentType.Parse(MimeTypes.Application.Csv),
            EmailAttachmentType.Doc => ContentType.Parse(MimeTypes.Application.Doc),
            EmailAttachmentType.Docx => ContentType.Parse(MimeTypes.Application.Docx),
            EmailAttachmentType.Pdf => ContentType.Parse(MimeTypes.Application.Pdf),
            EmailAttachmentType.Xls => ContentType.Parse(MimeTypes.Application.Xls),
            EmailAttachmentType.Xlsx => ContentType.Parse(MimeTypes.Application.Xlsx),
            EmailAttachmentType.Zip => ContentType.Parse(MimeTypes.Application.Zip),
            EmailAttachmentType.ZipWindows => ContentType.Parse(MimeTypes.Application.ZipWindows),
            EmailAttachmentType.Rfc822 => ContentType.Parse(MimeTypes.Images.Gif),
            _ => throw new ArgumentOutOfRangeException(nameof(attachmentType), attachmentType, null)
        };
    }

    public static TextPart ToTextPart(this EmailBody emailBody)
        => new(emailBody.ContentType == EmailContentType.Html ? "html" : "plain") { Text = emailBody.Message };
}

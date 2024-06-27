namespace Wiknap.Email.Models;

public record EmailMessage
{
    public string? Subject { get; set; }
    public HashSet<Recipient> Recipients { get; } = [];
    public EmailBody? Body { get; set; }
    public HashSet<EmailAttachment> Attachments { get; } = [];
}

public record EmailAttachment(string Filename, EmailAttachmentType Type, Stream Content);

public enum EmailAttachmentType
{
    Gif,
    Png
}

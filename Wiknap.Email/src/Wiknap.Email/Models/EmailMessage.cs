namespace Wiknap.Email.Models;

public record EmailMessage
{
    public string? Subject { get; set; }
    public HashSet<Recipient> Recipients { get; } = [];
    public EmailBody? Body { get; set; }
    public HashSet<EmailAttachment> Attachments { get; } = [];
}

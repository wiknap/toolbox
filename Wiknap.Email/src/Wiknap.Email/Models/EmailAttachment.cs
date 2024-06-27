namespace Wiknap.Email.Models;

public record EmailAttachment(string Filename, EmailAttachmentType Type, Stream Content);

namespace Wiknap.Email.Models;

public record EmailMessage : IAsyncDisposable
{
    public string? Subject { get; set; }
    public HashSet<Recipient> Recipients { get; } = [];
    public EmailBody? Body { get; set; }
    public HashSet<EmailAttachment> Attachments { get; } = [];

    public async ValueTask DisposeAsync()
    {
        foreach (var attachment in Attachments)
            await attachment.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}

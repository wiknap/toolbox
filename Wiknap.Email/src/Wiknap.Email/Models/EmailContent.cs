namespace Wiknap.Email.Models;

public sealed record EmailContent(EmailBody? Body, IReadOnlyList<EmailAttachment> Attachments) : IAsyncDisposable
{
    public EmailContent(EmailBody? Body) : this(Body, [])
    {}

    public async ValueTask DisposeAsync()
    {
        foreach (var attachment in Attachments)
            await attachment.DisposeAsync();
    }
}

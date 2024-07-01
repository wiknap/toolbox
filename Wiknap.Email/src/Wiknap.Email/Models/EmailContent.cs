using System.Collections.Immutable;

namespace Wiknap.Email.Models;

public record EmailContent(EmailBody? Body, IReadOnlyList<EmailAttachment> Attachments) : IAsyncDisposable
{
    public EmailContent(EmailBody? Body) : this(Body, ImmutableArray<EmailAttachment>.Empty)
    {}

    public async ValueTask DisposeAsync()
    {
        foreach (var attachment in Attachments)
        {
            await attachment.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}

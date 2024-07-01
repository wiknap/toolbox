namespace Wiknap.Email.Models;

public record EmailAttachment(string Filename, EmailAttachmentType Type, Stream Content) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await Content.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}

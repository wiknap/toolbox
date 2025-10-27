namespace Wiknap.Email.Models;

public sealed record EmailAttachment(string Filename, EmailAttachmentType Type, Stream Content) : IAsyncDisposable
{
    public ValueTask DisposeAsync() => Content.DisposeAsync();
}

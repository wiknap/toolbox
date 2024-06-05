using JetBrains.Annotations;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;

using MimeKit;

namespace Wiknap.Email;

[PublicAPI]
public class EmailClient : IEmailClient
{
    private readonly IEmailClientConfiguration configuration;
    private readonly MailboxAddress senderMailboxAddress;

    public EmailClient(IEmailClientConfiguration configuration)
    {
        this.configuration = configuration;
        senderMailboxAddress = new MailboxAddress(this.configuration.SenderName, this.configuration.Login);
    }

    public Task SendEmailAsync(string mailTo, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
        => SendEmailAsync([new Recipient(mailTo)], subject, message, isHtml, ct);

    public async Task SendEmailAsync(Recipient[] recipients, string subject, string message, bool isHtml = false,
        CancellationToken ct = default)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(senderMailboxAddress);
        mimeMessage.AddRecipients(recipients);
        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart(isHtml ? "html" : "plain") { Text = message };

        using var client = await GetSmtpClientAsync(ct).ConfigureAwait(false);
        await client.SendAsync(mimeMessage, ct).ConfigureAwait(false);
        await client.DisconnectAsync(true, ct).ConfigureAwait(false);
    }

    public async Task<string?> GetEmailContentAsync(SearchParameters parameters,
        EmailContentType? contentType = null, CancellationToken ct = default)
    {
        using var client = await GetImapClientAsync(ct).ConfigureAwait(false);
        await client.Inbox.OpenAsync(FolderAccess.ReadOnly, ct).ConfigureAwait(false);
        var searchResult = await client.Inbox.SearchAsync(GetSearchQuery(parameters), ct).ConfigureAwait(false);
        if (searchResult.Count == 0)
            return null;

        var id = searchResult.Last();
        var message = await client.Inbox.GetMessageAsync(id, ct).ConfigureAwait(false);
        if (parameters.DeliveredAfter.HasValue && message.Date <= new DateTimeOffset(parameters.DeliveredAfter.Value))
            return null;

        return message?.GetMessageContent(contentType);
    }

    private static SearchQuery GetSearchQuery(SearchParameters parameters)
    {
        var queries = new List<SearchQuery>();

        if (parameters.SenderEmail is not null)
            queries.Add(SearchQuery.FromContains(parameters.SenderEmail));

        if (parameters.Subject is not null)
            queries.Add(SearchQuery.SubjectContains(parameters.Subject));

        if (parameters.DeliveredAfter.HasValue)
            queries.Add(SearchQuery.DeliveredAfter(parameters.DeliveredAfter.Value));

        SearchQuery? resultQuery = null;
        foreach (var query in queries)
        {
            if (resultQuery is null)
            {
                resultQuery = query;
                continue;
            }

            resultQuery = resultQuery.And(query);
        }

        return resultQuery ?? SearchQuery.All;
    }

    private async Task<SmtpClient> GetSmtpClientAsync(CancellationToken ct)
    {
        var client = new SmtpClient();
        await ConnectAndAuthenticateAsync(client, configuration.SmtpHost, configuration.SmtpPort, ct)
            .ConfigureAwait(false);
        return client;
    }

    private async Task<ImapClient> GetImapClientAsync(CancellationToken ct)
    {
        var client = new ImapClient();
        await ConnectAndAuthenticateAsync(client, configuration.ImapHost, configuration.ImapPort, ct)
            .ConfigureAwait(false);
        return client;
    }

    private async Task ConnectAndAuthenticateAsync(IMailService service, string host, int port,
        CancellationToken ct = default)
    {
        await service.ConnectAsync(host, port, cancellationToken: ct).ConfigureAwait(false);
        await service.AuthenticateAsync(configuration.Login, configuration.Password, ct).ConfigureAwait(false);
    }
}

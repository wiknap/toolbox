using JetBrains.Annotations;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;

using MimeKit;

using Wiknap.Email.Models;

namespace Wiknap.Email;

[PublicAPI]
public class EmailClient : IEmailClient
{
    private readonly IEmailClientConfiguration _configuration;
    private readonly MailboxAddress _senderMailboxAddress;

    public EmailClient(IEmailClientConfiguration configuration)
    {
        _configuration = configuration;
        var address = !string.IsNullOrEmpty(_configuration.SenderEmail)
            ? _configuration.SenderEmail
            : _configuration.Login;
        _senderMailboxAddress = new MailboxAddress(_configuration.SenderName, address);
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct = default)
    {
        using var mimeMessage = message.ToMimeMessage(_senderMailboxAddress);
        using var client = await GetSmtpClientAsync(ct).ConfigureAwait(false);
        await client.SendAsync(mimeMessage, ct).ConfigureAwait(false);
        await client.DisconnectAsync(true, ct).ConfigureAwait(false);
    }

    public async Task<EmailContent?> GetEmailContentAsync(SearchParameters parameters, CancellationToken ct = default)
    {
        using var client = await GetImapClientAsync(ct).ConfigureAwait(false);
        await client.Inbox.OpenAsync(FolderAccess.ReadOnly, ct).ConfigureAwait(false);
        var searchResult = await client.Inbox.SearchAsync(GetSearchQuery(parameters), ct).ConfigureAwait(false);
        if (searchResult.Count == 0)
            return null;

        var id = searchResult.Last();
        var message = await client.Inbox.GetMessageAsync(id, ct).ConfigureAwait(false);
        if (parameters.DeliveredAfter.HasValue && IsMessageDeliveredAfter(parameters.DeliveredAfter.Value, message))
            return null;

        return message?.GetEmailContent();
    }

    private static bool IsMessageDeliveredAfter(DateTimeOffset dto, MimeMessage message)
        => message.Date <= dto.TrimMilliseconds();

    private static SearchQuery GetSearchQuery(SearchParameters parameters)
    {
        var queries = new List<SearchQuery>();

        if (parameters.SenderEmail is not null)
            queries.Add(SearchQuery.FromContains(parameters.SenderEmail));

        if (parameters.Subject is not null)
            queries.Add(SearchQuery.SubjectContains(parameters.Subject));

        if (parameters.DeliveredAfter.HasValue)
        {
            var dateTime = parameters.DeliveredAfter.Value.UtcDateTime.TrimMilliseconds();
            queries.Add(SearchQuery.DeliveredAfter(dateTime));
        }

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
        await ConnectAndAuthenticateAsync(client, _configuration.SmtpHost, _configuration.SmtpPort, ct)
            .ConfigureAwait(false);
        return client;
    }

    private async Task<ImapClient> GetImapClientAsync(CancellationToken ct)
    {
        var client = new ImapClient();
        await ConnectAndAuthenticateAsync(client, _configuration.ImapHost, _configuration.ImapPort, ct)
            .ConfigureAwait(false);
        return client;
    }

    private async Task ConnectAndAuthenticateAsync(IMailService service, string host, int port,
        CancellationToken ct = default)
    {
        await service.ConnectAsync(host, port, cancellationToken: ct).ConfigureAwait(false);
        await service.AuthenticateAsync(_configuration.Login, _configuration.Password, ct).ConfigureAwait(false);
    }
}

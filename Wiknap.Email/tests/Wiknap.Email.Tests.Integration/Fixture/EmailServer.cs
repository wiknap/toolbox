using Wiknap.Testcontainers.MailServer;

using Xunit;

namespace Wiknap.Email.Tests.Integration.Fixture;

public sealed class EmailServer : IAsyncLifetime
{
    public const string AdminEmail = "admin@example.com";
    public const string DefaultPassword = "passwd123";
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly HashSet<string> _addedUsers = [];

    private readonly MailServerContainer _mailServerContainer = new MailServerBuilder()
        .WithAdminEmail(AdminEmail)
        .WithAdminPassword(DefaultPassword)
        .Build();

    public ushort SmtpPort => _mailServerContainer.SmtpPort;
    public ushort ImapPort => _mailServerContainer.ImapPort;
    public static string Host => MailServerBuilder.Host;

    public async ValueTask InitializeAsync()
        => await _mailServerContainer.StartAsync(_cancellationTokenSource.Token).ConfigureAwait(false);

    public Task AddUserAsync(string email)
    {
        return !_addedUsers.Add(email)
            ? Task.CompletedTask
            : _mailServerContainer.AddEmailAsync(email, DefaultPassword, CancellationToken.None);
    }

    public async ValueTask DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
        await _mailServerContainer.StopAsync().ConfigureAwait(false);
    }
}

[CollectionDefinition("EmailServer")]
public class EmailServerCollection : ICollectionFixture<EmailServer>;

using Wiknap.Testcontainers.MailServer;

using Xunit;

namespace Wiknap.Email.Tests.Integration.Fixture;

public sealed class EmailServer : IAsyncLifetime
{
    public const string UserEmail = "user@example.com";
    public const string UserPassword = "passwd123";
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly MailServerContainer _mailServerContainer = new MailServerBuilder()
        .Build();

    public ushort SmtpPort => _mailServerContainer.SmtpPort;
    public ushort ImapPort => _mailServerContainer.ImapPort;
    public static string Host => MailServerBuilder.Host;
    public string AdminEmail => _mailServerContainer.AdminEmail;
    public string AdminPassword => _mailServerContainer.AdminPassword;

    public async Task InitializeAsync()
    {
        await _mailServerContainer.StartAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
        await _mailServerContainer.AddEmailAsync(UserEmail, UserPassword, _cancellationTokenSource.Token)
            .ConfigureAwait(false);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
        await _mailServerContainer.StopAsync().ConfigureAwait(false);
    }
}

[CollectionDefinition("EmailServer")]
public class EmailServerCollection : ICollectionFixture<EmailServer>;

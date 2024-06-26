using Wiknap.Testcontainers.MailServer;

using Xunit;

namespace Wiknap.Email.Tests.Integration.Fixture;

public sealed class EmailServer : IAsyncLifetime
{
    public const string UserEmail = "user@example.com";
    public const string UserPassword = "passwd123";
    private readonly CancellationTokenSource cancellationTokenSource = new();

    private readonly MailServerContainer mailServerContainer = new MailServerBuilder()
        .Build();

    public ushort SmtpPort => mailServerContainer.SmtpPort;
    public ushort ImapPort => mailServerContainer.ImapPort;
    public static string Host => MailServerBuilder.Host;
    public string AdminEmail => mailServerContainer.AdminEmail;
    public string AdminPassword => mailServerContainer.AdminPassword;

    public async Task InitializeAsync()
    {
        await mailServerContainer.StartAsync(cancellationTokenSource.Token).ConfigureAwait(false);
        await mailServerContainer.AddEmailAsync(UserEmail, UserPassword, cancellationTokenSource.Token)
            .ConfigureAwait(false);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await cancellationTokenSource.CancelAsync().ConfigureAwait(false);
        await mailServerContainer.StopAsync().ConfigureAwait(false);
    }
}

[CollectionDefinition("EmailServer")]
public class EmailServerCollection : ICollectionFixture<EmailServer>;

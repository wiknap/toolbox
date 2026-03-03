using System.Diagnostics;

using Bogus;

using Wiknap.Email.Models;

using Xunit;

namespace Wiknap.Email.Tests.Integration.Fixture;

[Collection("EmailServer")]
public abstract class IntegrationTestsBase : IAsyncLifetime
{
    private const string Domain = "example.com";
    private readonly EmailServer _emailServer;
    protected readonly IEmailClient EmailClient;
    protected readonly IEmailClient UserEmailClient;
    protected readonly Faker Faker = new();

    protected const string UserEmail = $"user@{Domain}";
    protected readonly string AdminEmail;

    protected IntegrationTestsBase(EmailServer emailServer)
    {
        _emailServer = emailServer;
        AdminEmail = EmailServer.AdminEmail;
        var config = CreateConfig(EmailServer.AdminEmail);
        EmailClient = new Email.EmailClient(config);
        var userConfig = CreateConfig(UserEmail);
        UserEmailClient = new Email.EmailClient(userConfig);
    }

    public async ValueTask InitializeAsync() => await _emailServer.AddUserAsync(UserEmail);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    protected string GetNewEmail() => Faker.Internet.Email(provider: Domain);

    private TestEmailClientConfiguration CreateConfig(string email)
    {
        return new TestEmailClientConfiguration(EmailServer.Host, _emailServer.SmtpPort, EmailServer.Host,
            _emailServer.ImapPort, email, EmailServer.DefaultPassword);
    }

    protected async Task SendEmailAsync(string email, EmailMessage message)
    {
        await _emailServer.AddUserAsync(email);
        var config = CreateConfig(email);
        var client = new Email.EmailClient(config);
        await client.SendEmailAsync(message);
    }

    protected async Task<EmailContent?> GetUserEmailContentAsync(SearchParameters searchParameters)
    {
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < TimeSpan.FromSeconds(3))
        {
            var content = await UserEmailClient
                .GetEmailContentAsync(searchParameters).ConfigureAwait(false);

            if (content is not null)
                return content;
        }

        return null;
    }

    protected async Task<IReadOnlyCollection<ReceivedEmailMessage>> GetEmailsAsync(SearchParameters searchParameters, int expectedCount)
    {
        var stopwatch = Stopwatch.StartNew();

        IReadOnlyCollection<ReceivedEmailMessage> messages = [];
        while (stopwatch.Elapsed < TimeSpan.FromSeconds(3))
        {
            messages = await UserEmailClient
                .GetEmailsAsync(searchParameters).ConfigureAwait(false);

            if (messages.Count >= expectedCount)
                return messages;
        }

        return messages;
    }
}

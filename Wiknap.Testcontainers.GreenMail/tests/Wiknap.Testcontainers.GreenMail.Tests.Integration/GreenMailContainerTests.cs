using System.Diagnostics;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;

using MimeKit;

using Shouldly;

using Xunit;

namespace Wiknap.Testcontainers.GreenMail.Tests.Integration;

public sealed class GreenMailContainerTests : IDisposable
{
    private const string SenderEmail = "sender@example.com";
    private const string ReceiverEmail = "receiver@example.com";
    private const string Password = "passwd123";
    private const string Subject = "Subject";
    private const string Content = "Content";

    private readonly CancellationTokenSource _cts = new();

    [Fact]
    public async Task Given_DefaultConfiguration_When_SendEmail_Then_EmailIsInReceiverInbox()
    {
        // Arrange
        await using var container = new GreenMailBuilder().Build();
        await container.StartAsync(_cts.Token);

        // Act
        await SendEmailAsync(container, SenderEmail, Password, ReceiverEmail);

        // Assert
        var content = await GetEmailContentAsync(container, ReceiverEmail, Password);
        content.ShouldNotBeNull();
        content.Trim().ShouldBe(Content);
    }

    [Fact]
    public async Task Given_AuthenticationEnabledAndUser_When_Authenticate_Then_Succeeds()
    {
        // Arrange
        await using var container = new GreenMailBuilder()
            .WithAuthenticationDisabled(false)
            .WithUser(SenderEmail, Password)
            .Build();
        await container.StartAsync(_cts.Token);
        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(container.Hostname, container.SmtpPort, SecureSocketOptions.None, _cts.Token);

        // Act & Assert
        await Should.NotThrowAsync(() => smtpClient.AuthenticateAsync(SenderEmail, Password, _cts.Token));
    }

    [Fact]
    public async Task Given_AuthenticationEnabledAndUser_When_AuthenticateWithInvalidPassword_Then_Throws()
    {
        // Arrange
        await using var container = new GreenMailBuilder()
            .WithAuthenticationDisabled(false)
            .WithUser(SenderEmail, Password)
            .Build();
        await container.StartAsync(_cts.Token);
        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(container.Hostname, container.SmtpPort, SecureSocketOptions.None, _cts.Token);

        // Act & Assert
        await Should.ThrowAsync<AuthenticationException>(
            () => smtpClient.AuthenticateAsync(SenderEmail, "invalid", _cts.Token));
    }

    [Fact]
    public async Task Given_AuthenticationEnabled_When_AddUserAsync_Then_UserCanReceiveEmail()
    {
        // Arrange
        await using var container = new GreenMailBuilder()
            .WithAuthenticationDisabled(false)
            .WithUser(SenderEmail, Password)
            .Build();
        await container.StartAsync(_cts.Token);

        // Act
        await container.AddUserAsync(ReceiverEmail, Password, _cts.Token);

        // Assert
        await SendEmailAsync(container, SenderEmail, Password, ReceiverEmail);
        var content = await GetEmailContentAsync(container, ReceiverEmail, Password);
        content.ShouldNotBeNull();
    }

    [Fact]
    public async Task Given_EmailInInbox_When_PurgeAsync_Then_InboxIsEmpty()
    {
        // Arrange
        await using var container = new GreenMailBuilder().Build();
        await container.StartAsync(_cts.Token);
        await SendEmailAsync(container, SenderEmail, Password, ReceiverEmail);
        (await GetEmailContentAsync(container, ReceiverEmail, Password)).ShouldNotBeNull();

        // Act
        await container.PurgeAsync(_cts.Token);

        // Assert
        using var imapClient = await ConnectImapAsync(container, ReceiverEmail, Password);
        await imapClient.Inbox.OpenAsync(FolderAccess.ReadOnly, _cts.Token);
        imapClient.Inbox.Count.ShouldBe(0);
    }

    private async Task SendEmailAsync(GreenMailContainer container, string email, string password,
        string receiverEmail)
    {
        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(container.Hostname, container.SmtpPort, SecureSocketOptions.None, _cts.Token);
        await smtpClient.AuthenticateAsync(email, password, _cts.Token);
        using var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(email, email));
        mimeMessage.To.Add(new MailboxAddress(receiverEmail, receiverEmail));
        mimeMessage.Subject = Subject;
        mimeMessage.Body = new TextPart("plain") { Text = Content };
        await smtpClient.SendAsync(mimeMessage, _cts.Token);
        await smtpClient.DisconnectAsync(true, _cts.Token);
    }

    private async Task<string?> GetEmailContentAsync(GreenMailContainer container, string email, string password)
    {
        using var imapClient = await ConnectImapAsync(container, email, password);
        await imapClient.Inbox.OpenAsync(FolderAccess.ReadOnly, _cts.Token);
        var stopWatch = Stopwatch.StartNew();

        while (stopWatch.Elapsed <= TimeSpan.FromSeconds(10))
        {
            var searchResult = await imapClient.Inbox.SearchAsync(SearchQuery.SubjectContains(Subject), _cts.Token);
            if (searchResult.Count == 0)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100), _cts.Token);
                continue;
            }

            var message = await imapClient.Inbox.GetMessageAsync(searchResult[^1], _cts.Token);
            return message.TextBody;
        }

        return null;
    }

    private async Task<ImapClient> ConnectImapAsync(GreenMailContainer container, string email, string password)
    {
        var imapClient = new ImapClient();
        await imapClient.ConnectAsync(container.Hostname, container.ImapPort, SecureSocketOptions.None, _cts.Token);
        await imapClient.AuthenticateAsync(email, password, _cts.Token);
        return imapClient;
    }

    public void Dispose() => _cts.Dispose();
}

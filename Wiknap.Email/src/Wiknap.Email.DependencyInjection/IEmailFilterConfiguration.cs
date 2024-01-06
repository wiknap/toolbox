namespace Wiknap.Email.DependencyInjection;

public interface IEmailFilterConfiguration
{
    bool ExcludeAll { get; }
    IEnumerable<EmailRule> Exclude { get; }
    IEnumerable<EmailRule> Include { get; }
}

public record EmailRule(EmailRuleType Type, string Value);

public enum EmailRuleType
{
    Domain,
    Email
}
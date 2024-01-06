namespace Wiknap.Email.DependencyInjection;

public static class EmailRuleExtensions
{
    public static bool Validate(this EmailRule rule, string mailToCheck)
    {
        return rule.Type switch
        {
            EmailRuleType.Domain => ContainsDomain(mailToCheck, rule.Value),
            EmailRuleType.Email => mailToCheck.Equals(rule.Value, StringComparison.OrdinalIgnoreCase),
            _ => throw new ArgumentOutOfRangeException(nameof(rule.Type))
        };
    }

    private static bool ContainsDomain(string mailToCheck, string domain)
    {
        var split = mailToCheck.Split('@');
        return split.Length >= 2 && split[1].Equals(domain, StringComparison.OrdinalIgnoreCase);
    }
}
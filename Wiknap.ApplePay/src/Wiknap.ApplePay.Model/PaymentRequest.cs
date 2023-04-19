using System.Text.Json.Serialization;

namespace Wiknap.ApplePay.Model;

public record PaymentRequest(
    MerchantCapability[] MerchantCapabilities,
    string[] SupportedNetworks,
    string CountryCode,
    string CurrencyCode,
    LineItem Total,
    HashSet<ContactField>? RequiredBillingContactFields = null,
    PaymentContact? BillingContact = null,
    HashSet<ContactField>? RequiredShippingContactFields = null,
    PaymentContact? ShippingContact = null,
    string? ApplicationData = null,
    string[]? SupportedCountries = null,
    bool? SupportsCouponCode = null,
    string? CouponCode = null,
    ShippingContactEditingMode? ShippingContactEditingMode = null,
    LineItem[]? LineItems = null,
    ShippingType? ShippingType = null,
    ShippingMethod[]? ShippingMethods = null,
    PaymentTokenContext[]? MultiTokenContexts = null,
    AutomaticReloadPaymentRequest? AutomaticReloadPaymentRequest = null,
    RecurringPaymentRequest? RecurringPaymentRequest = null);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MerchantCapability
{
    supports3DS,
    supportsEMV,
    supportsCredit,
    supportsDebit
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContactField
{
    email,
    name,
    phone,
    postalAddress
}

public record PaymentContact(string PhoneNumber,
    string EmailAddress,
    string GivenName,
    string FamilyName,
    string PhoneticGivenName,
    string PhoneticFamilyName,
    string[]? AddressLines,
    string SubLocality,
    string Locality,
    string PostalCode,
    string SubAdministrativeArea,
    string AdministrativeArea,
    string Country,
    string CountryCode);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShippingContactEditingMode
{
    enabled,
    storePickup
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShippingType
{
    shipping,
    delivery,
    storePickup,
    servicePickup
}

public record ShippingMethod(string Label, string Detail, string Amount, string? Identifier,
    DateComponentsRange? DateComponentsRange = null);

public record DateComponentsRange(DateComponents StartDateComponents, DateComponents EndDateComponents);

public record DateComponents(long Years, long Months, long Days, long Hours);

public record PaymentTokenContext(string MerchantIdentifier, string ExternalIdentifier, string MerchantName,
    string Amount, string? MerchantDomain);

public record AutomaticReloadPaymentRequest(string PaymentDescription, LineItem AutomaticReloadBilling,
    string ManagementUrl, string? BillingAgreement, string? TokenNotificationUrl);

public record RecurringPaymentRequest(string PaymentDescription, LineItem RegularBilling, string ManagementUrl,
    string TokenNotificationUrl, LineItem? TrialBilling, string? BillingAgreement);
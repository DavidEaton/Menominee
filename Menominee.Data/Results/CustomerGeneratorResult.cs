using Menominee.Domain.Entities;

namespace Menominee.Data.Results
{
    public static class CustomerGeneratorResult
    {
        public static IReadOnlyList<Customer> Customers { get; set; } = new List<Customer>();

        // If you also generate related entities like vehicles, emails, and phones,
        // you can add properties for them here, similar to how VendorGeneratorResult has PaymentMethods and DefaultPaymentMethods.
        // Example:
        // public static IReadOnlyList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        // public static IReadOnlyList<Email> Emails { get; set; } = new List<Email>();
        // public static IReadOnlyList<Phone> Phones { get; set; } = new List<Phone>();
    }
}

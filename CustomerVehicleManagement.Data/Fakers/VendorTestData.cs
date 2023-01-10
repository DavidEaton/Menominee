using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Data.Fakers
{
    public static class VendorTestData
    {
        //public static IEnumerable<Vendor> GenerateVendors(int count)
        //{
        //   return new Faker<Vendor>()
        //        .RuleFor(vendor => vendor.Name, faker => faker.Company.CompanyName())
        //        .RuleFor(vendor => vendor.VendorCode, faker => faker.Random.AlphaNumeric(5))
        //        .RuleFor(vendor => vendor.VendorRole, faker => faker.PickRandom<VendorRole>())
        //        .RuleFor(vendor => vendor.IsActive, faker => faker.Random.Bool())
        //        .RuleFor(vendor => vendor.Address, faker => AddressFaker.AddressTestData)
        //        .RuleFor(vendor => vendor.Emails, faker => EmailTestData.GenerateEmails(2))
        //        .RuleFor(vendor => vendor.Phones, faker => PhoneTestData.GeneratePhones(2))
        //        .RuleFor(vendor => vendor.DefaultPaymentMethod, faker => DefaultPaymentMethodTestData.GenerateDefaultPayment);
        //}
    }
}
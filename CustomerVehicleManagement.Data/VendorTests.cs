using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Data
{
    public class VendorTests
    {
        public void GenerateTestData()
        {
            var addressFaker = new Faker<Address>()
                .RuleFor(a => a.AddressLine, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.PickRandom<State>())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());

            var phoneFaker = new Faker<Phone>()
                .RuleFor(p => p.Number, f => f.Phone.PhoneNumber())
                .RuleFor(p => p.IsPrimary, f => f.Random.Bool());

            var emailFaker = new Faker<Email>()
                .RuleFor(e => e.Address, f => f.Internet.Email())
                .RuleFor(e => e.IsPrimary, f => f.Random.Bool());

            var vendorFaker = new Faker<Vendor>()
                .RuleFor(v => v.Name, f => f.Company.CompanyName())
                .RuleFor(v => v.Address, f => addressFaker.Generate())
                .RuleFor(v => v.Phones, f => phoneFaker.Generate(3).ToList())
                .RuleFor(v => v.Emails, f => emailFaker.Generate(2).ToList())
                .RuleFor(v => v.VendorRole, f => f.PickRandom<VendorRole>());

            var vendors = vendorFaker.Generate(10);
        }
    }
}

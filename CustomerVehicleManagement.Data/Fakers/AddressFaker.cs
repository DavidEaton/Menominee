using Bogus;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Data.Fakers
{
    public static class AddressFaker
    {
        public static Faker<Address> AddressTestData { get; set; } = new Faker<Address>()
                .RuleFor(a => a.AddressLine, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.PickRandom<State>())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());

        //public static Faker<Address> AddressMoopsData()
        //{
        //    return new Faker<Address>()
        //        .RuleFor(a => a.AddressLine, f => f.Address.StreetAddress())
        //        .RuleFor(a => a.City, f => f.Address.City())
        //        .RuleFor(a => a.State, f => f.PickRandom<State>())
        //        .RuleFor(a => a.PostalCode, f => f.Address.ZipCode());
        //}
    }
}

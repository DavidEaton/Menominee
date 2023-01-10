using Bogus;
using CustomerVehicleManagement.Data.Fakers;


var faker = new Faker();
var paymentMethodNames = faker.Make(10, () => faker.Random.Words());


var vendors = VendorFaker.MakeVendorFakes(paymentMethodNames, 5);

foreach (var vendor in vendors)
{
    Console.WriteLine($"Id: {vendor.Id}");
    Console.WriteLine($"Name: {vendor.Name}");
    Console.WriteLine($"VendorCode: {vendor.VendorCode}");
    Console.WriteLine($"VendorRole: {vendor.VendorRole}");
    Console.WriteLine($"Notes: {vendor.Note}");
    Console.WriteLine($"IsActive: {vendor.IsActive}");
}

Console.ReadLine();
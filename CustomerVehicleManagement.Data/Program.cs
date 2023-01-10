using Bogus;
using CustomerVehicleManagement.Data.Fakers;

var random = new Random();
var CountOfVendorsToCreate = 24;
var CountOfPaymentMethodsToCreate = CountOfVendorsToCreate;

var faker = new Faker();
var vendors = VendorFaker.MakeVendorFakes(CountOfVendorsToCreate);
var paymentMethodNames = faker.Make(CountOfVendorsToCreate / 2, () => faker.Random.Words());
var paymentMethods = VendorFaker.MakePaymentMethodFakes(
    paymentMethodNames: paymentMethodNames,
    count: CountOfPaymentMethodsToCreate);
var defaultPaymentMethods = VendorFaker.MakeDefaultPaymentMethodFakes(
    paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
    count: CountOfPaymentMethodsToCreate/2);
defaultPaymentMethods.AddRange(VendorFaker.MakeDefaultPaymentMethodFakes(
    paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
    count: CountOfPaymentMethodsToCreate / 2));

// Randomly assign to some Vendors: DefaultPaymentMethod
foreach (var vendor in vendors)
    if (faker.Random.Bool())
        vendor.SetDefaultPaymentMethod(defaultPaymentMethods[faker.Random.Int(0, defaultPaymentMethods.Count - 1)]);

foreach (var vendor in vendors)
{
    Console.WriteLine($"Vendor Name: {vendor.Name}");
    Console.WriteLine($"DefaultPaymentMethod Name: {vendor?.DefaultPaymentMethod?.PaymentMethod.Name}");
}


// TODO: Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor

Console.ReadLine();

//foreach (var vendor in vendors)
//{
//    Console.WriteLine($"Id: {vendor.Id}");
//    Console.WriteLine($"Name: {vendor.Name}");
//    Console.WriteLine($"VendorCode: {vendor.VendorCode}");
//    Console.WriteLine($"VendorRole: {vendor.VendorRole}");
//    Console.WriteLine($"Notes: {vendor.Note}");
//    Console.WriteLine($"IsActive: {vendor.IsActive}");
//}

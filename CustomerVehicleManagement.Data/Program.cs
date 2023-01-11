using Bogus;
using CustomerVehicleManagement.Data.Fakers;
using CustomerVehicleManagement.Domain.Entities.Payables;

var random = new Random();
var CountOfVendorsToCreate = 25;
var CountOfPaymentMethodsToCreate = CountOfVendorsToCreate;

var faker = new Faker();
var vendors = VendorFaker.MakeVendorFakes(CountOfVendorsToCreate);
var paymentMethodNames = faker.Make(CountOfVendorsToCreate / 2, () => faker.Random.Words());
var paymentMethods = VendorFaker.MakePaymentMethodFakes(
    paymentMethodNames: paymentMethodNames,
    count: CountOfPaymentMethodsToCreate);
var defaultPaymentMethods = VendorFaker.MakeDefaultPaymentMethodFakes(
    paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
    count: CountOfPaymentMethodsToCreate / 2);
defaultPaymentMethods.AddRange(VendorFaker.MakeDefaultPaymentMethodFakes(
    paymentMethod: paymentMethods[random.Next(0, CountOfPaymentMethodsToCreate - 1)],
    count: CountOfPaymentMethodsToCreate / 2));

// Randomly assign to some Vendors: DefaultPaymentMethod
foreach (var vendor in vendors)
    if (faker.Random.Bool())
    {
        random = new Random();
        vendor.SetDefaultPaymentMethod(defaultPaymentMethods[faker.Random.Int(0, defaultPaymentMethods.Count - 1)]);
    }

// TODO: Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor
foreach (var paymentMethod in paymentMethods)
{
    random = new Random();
    paymentMethod.SetReconcilingVendor(vendors[random.Next(0, CountOfPaymentMethodsToCreate - 1)]);
}

foreach (var vendor in vendors)
{
    Console.WriteLine($"Vendor Name: {vendor.Name}");
    Console.WriteLine($"DefaultPaymentMethod Name: {vendor?.DefaultPaymentMethod?.PaymentMethod.Name}");
    Console.WriteLine($"DefaultPaymentMethod ReconcilingVendor Name: {vendor?.DefaultPaymentMethod?.PaymentMethod.ReconcilingVendor.Name}");
}



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

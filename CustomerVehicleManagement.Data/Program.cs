using Bogus;
using CustomerVehicleManagement.Data.Fakers;


var faker = new Faker();
var paymentMethodNames = faker.Make(10, () => faker.Random.Words());
var vendors = VendorFaker.MakeVendorFakes(paymentMethodNames, 5);
var paymentMethod = VendorFaker.MakeVendorInvoicePaymentMethodFake(paymentMethodNames);
var defaultPaymentMethod = VendorFaker.MakeDefaultPaymentMethodFake(paymentMethod);

// Randomly assign to some Vendors: DefaultPaymentMethod
foreach (var vendor in vendors)
{
    if (faker.Random.Bool())
        vendor.SetDefaultPaymentMethod(defaultPaymentMethod);
}

foreach (var vendor in vendors)
{
    Console.WriteLine($"Vendor Name: {vendor.Name}");
    Console.WriteLine($"DefaultPaymentMethod Name: {vendor?.DefaultPaymentMethod?.PaymentMethod.Name}");
}


// Randomly assign to some VendorInvoicePaymentMethods: Vendor reconcilingVendor

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

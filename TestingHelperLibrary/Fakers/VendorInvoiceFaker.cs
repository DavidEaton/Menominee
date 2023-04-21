using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;
using System;
using System.Linq;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoiceFaker : Faker<VendorInvoice>
    {
        public VendorInvoiceFaker(bool generateId = false, bool createCollections = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var status = faker.PickRandom<VendorInvoiceStatus>();
                var documentType = faker.PickRandom<VendorInvoiceDocumentType>();
                var total = Math.Round(faker.Random.Double(1, 10000), 2);
                var vendorInvoiceNumbers = faker.MakeLazy(5, () => faker.Random.String2(10)).ToList();
                var vendor = new VendorFaker(generateId).Generate();
                var invoiceNumber = GenerateInvoiceNumber(faker);
                var lineItems = new VendorInvoiceLineItemFaker(generateId: generateId).GenerateBetween(1, 5);
                var payments = new VendorInvoicePaymentFaker(generateId: generateId).GenerateBetween(1, 5);
                var taxes = new VendorInvoiceTaxFaker(generateId: generateId).GenerateBetween(1, 5);

                var result = VendorInvoice.Create(vendor, status, documentType, total, vendorInvoiceNumbers, invoiceNumber);

                VendorInvoice invoice = null;

                if (result.IsSuccess)
                    invoice = result.Value;

                if (result.IsFailure)
                    throw new InvalidOperationException(result.Error);

                if (createCollections)
                {
                    foreach (var line in lineItems)
                        invoice.AddLineItem(line);

                    foreach (var payment in payments)
                        invoice.AddPayment(payment);

                    foreach (var tax in taxes)
                        invoice.AddTax(tax);
                }

                return invoice;
            });
        }

        public static string GenerateInvoiceNumber(Faker faker)
        {
            var prefix = faker.Random.AlphaNumeric(3).ToUpper();
            var sequenceNumber = faker.Random.Number(1000, 9999);
            return $"{prefix}-{DateTime.Today.Day:00}{DateTime.Today.Month:00}{DateTime.Today.Year}-{sequenceNumber}";
        }
    }
}

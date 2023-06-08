using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoiceFaker : Faker<VendorInvoice>
    {
        public VendorInvoiceFaker(bool generateId = false, int lineItemsCount = 0, int paymentsCount = 0, int taxesCount = 0)
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
                var lineItems = new VendorInvoiceLineItemFaker(generateId: generateId).Generate(lineItemsCount);
                var payments = new VendorInvoicePaymentFaker(generateId: generateId).Generate(paymentsCount);
                var taxes = new VendorInvoiceTaxFaker(generateId: generateId).Generate(taxesCount);

                var invoice = VendorInvoice.Create(vendor, status, documentType, total, vendorInvoiceNumbers, invoiceNumber).Value;

                lineItems.ForEach(line =>
                    invoice.AddLineItem(line));

                payments.ForEach(payment =>
                    invoice.AddPayment(payment));

                taxes.ForEach(tax =>
                    invoice.AddTax(tax));

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

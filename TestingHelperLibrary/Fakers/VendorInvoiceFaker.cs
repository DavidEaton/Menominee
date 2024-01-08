using Bogus;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Enums;
using Menominee.TestingHelperLibrary.Fakers;

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

                var lineItems = lineItemsCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(lineItemsCount)
                            .Select(id => new VendorInvoiceLineItemFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new VendorInvoiceLineItemFaker(generateId: false).Generate(lineItemsCount);

                var payments = paymentsCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(paymentsCount)
                            .Select(id => new VendorInvoicePaymentFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new VendorInvoicePaymentFaker(generateId: false).Generate(paymentsCount);

                var taxes = taxesCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(taxesCount)
                            .Select(id => new VendorInvoiceTaxFaker(generateId: false, id: id).Generate())
                            .ToList()
                        : new VendorInvoiceTaxFaker(generateId: false).Generate(taxesCount);

                var invoice = VendorInvoice.Create(vendor, status, documentType, total, vendorInvoiceNumbers, invoiceNumber).Value;

                lineItems?.ForEach(line =>
                    invoice.AddLineItem(line));

                payments?.ForEach(payment =>
                    invoice.AddPayment(payment));

                taxes?.ForEach(tax =>
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

        public static List<VendorInvoice> MakeVendorInvoiceFakes(
            int invoicesToGenerateCount,
            IReadOnlyList<Vendor> vendors,
            IReadOnlyList<string> vendorInvoiceNumbers)
        {
            var retries = 10;
            var success = false;
            var random = new Random();

            while (!success && retries > 0)
            {
                try
                {
                    return new Faker<VendorInvoice>()

                        .CustomInstantiator(faker =>
                        {
                            return VendorInvoice.Create(
                                vendor: vendors[random.Next(0, vendors.Count - 1)],
                                status: faker.PickRandom<VendorInvoiceStatus>(),
                                documentType: faker.PickRandom<VendorInvoiceDocumentType>(),
                                total: faker.Random.Double(),
                                vendorInvoiceNumbers: vendorInvoiceNumbers,
                                invoiceNumber: faker.Commerce.Ean8()
                            ).Value;

                        }).Generate(invoicesToGenerateCount);
                }
                catch (Exception)
                {
                    retries--;
                }
            }

            return new List<VendorInvoice>();
        }
    }
}

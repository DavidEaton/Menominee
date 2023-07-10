using Bogus;
using Menominee.Domain.Entities.Payables;
using Menominee.Common.Enums;

namespace Menominee.Data.Fakers
{
    public class VendorInvoiceFaker
    {
        public static List<VendorInvoice> MakeVendorInvoiceFakes(
            int invoicesToGenerateCount,
            IReadOnlyList<Vendor> vendors,
            IReadOnlyList<string> vendorInvoiceNumbers)
        {
            int retries = 10;
            bool success = false;
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

﻿using Bogus;
using Menominee.Domain.Entities.Payables;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoiceTaxFaker : Faker<VendorInvoiceTax>
    {
        public VendorInvoiceTaxFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var salesTax = new SalesTaxFaker(generateId, id).Generate();
                var amount = Math.Round(faker.Random.Double(1, 1000.0), 2);

                var result = VendorInvoiceTax.Create(salesTax, amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}

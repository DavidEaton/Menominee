using CustomerVehicleManagement.Domain.Entities.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class SalesTaxHelper
    {
        public static SalesTax CreateSalesTax(SalesTaxToWrite taxToWrite)
        {
            if (taxToWrite is null)
                return null;

            var tax = SalesTax.Create(
                taxToWrite.Description,
                taxToWrite.TaxType,
                taxToWrite.Order,
                taxToWrite.IsAppliedByDefault,
                taxToWrite.IsTaxable,
                taxToWrite.TaxIdNumber,
                taxToWrite.PartTaxRate,
                taxToWrite.LaborTaxRate).Value;

            taxToWrite.TaxedExciseFees = new();

            if (taxToWrite?.TaxedExciseFees?.Count > 0)
            {
                foreach (var fee in taxToWrite.TaxedExciseFees)
                {
                    // TODO - ExciseFee or SalesTaxTaxableExciseFee ???
                    //tax.TaxedExciseFees.Add(ExciseFeeHelper.Transform(fee));
                }
            }

            return tax;
        }

        public static SalesTaxToWrite CreateSalesTax(SalesTaxToRead taxToRead)
        {
            if (taxToRead is null)
                return null;

            var taxToWrite = new SalesTaxToWrite
            {
                Description = taxToRead.Description,
                TaxType = taxToRead.TaxType,
                Order = taxToRead.Order,
                IsAppliedByDefault = taxToRead.IsAppliedByDefault,
                IsTaxable = taxToRead.IsTaxable,
                TaxIdNumber = taxToRead.TaxIdNumber,
                PartTaxRate = taxToRead.PartTaxRate,
                LaborTaxRate = taxToRead.LaborTaxRate,
                TaxedExciseFees = new()
            };

            if (taxToRead?.TaxedExciseFees?.Count > 0)
            {
                foreach (var fee in taxToRead.TaxedExciseFees)
                {
                    // TODO - ExciseFee or SalesTaxTaxableExciseFee ???
                    //taxToWrite.TaxedExciseFees.Add(ExciseFeeHelper.Transform(fee));
                }
            }

            return taxToWrite;
        }

        public static SalesTaxToRead CreateSalesTax(SalesTax salesTax)
        {
            if (salesTax is null)
                return null;

            var taxToRead = new SalesTaxToRead
            {
                Id = salesTax.Id,
                Description = salesTax.Description,
                TaxType = salesTax.TaxType,
                Order = salesTax.Order,
                IsAppliedByDefault = salesTax.IsAppliedByDefault,
                IsTaxable = salesTax.IsTaxable,
                TaxIdNumber = salesTax.TaxIdNumber,
                PartTaxRate = salesTax.PartTaxRate,
                LaborTaxRate = salesTax.LaborTaxRate,
                TaxedExciseFees = new()
            };

            if (salesTax?.TaxedExciseFees?.Count > 0)
            {
                foreach (var fee in salesTax.TaxedExciseFees)
                {
                    // TODO - ExciseFee or SalesTaxTaxableExciseFee ???
                    //taxToRead.TaxedExciseFees.Add(ExciseFeeHelper.Transform(fee));
                }
            }

            return taxToRead;
        }

        public static SalesTaxToReadInList CreateSalesTaxInList(SalesTax tax)
        {
            return tax is null
                ? null
                : new()
                {
                    Id = tax.Id,
                    Description = tax.Description,
                    TaxType = tax.TaxType,
                    Order = tax.Order,
                    IsAppliedByDefault = tax.IsAppliedByDefault,
                    IsTaxable = tax.IsTaxable,
                    TaxIdNumber = tax.TaxIdNumber,
                    PartTaxRate = tax.PartTaxRate,
                    LaborTaxRate = tax.LaborTaxRate
                };
        }
    }
}

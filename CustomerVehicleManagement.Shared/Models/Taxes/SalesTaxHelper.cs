using CustomerVehicleManagement.Domain.Entities.Taxes;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class SalesTaxHelper
    {
        public static SalesTax ConvertWriteDtoToEntity(SalesTaxToWrite tax)
        {
            if (tax is null)
                return null;

            List<ExciseFee> fees = new();

            if (tax?.ExciseFees is not null && tax?.ExciseFees.Count > 0)
                fees = ExciseFeeHelper.CreateExciseFees(tax.ExciseFees);

            return SalesTax.Create(
                tax.Description,
                tax.TaxType,
                tax.Order,
                tax.IsAppliedByDefault,
                tax.IsTaxable,
                tax.TaxIdNumber,
                tax.PartTaxRate,
                tax.LaborTaxRate,
                fees)
                .Value;
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
                ExciseFees = new()
            };

            if (taxToRead?.ExciseFees?.Count > 0)
            {
                foreach (var fee in taxToRead.ExciseFees)
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
                ExciseFees = new()
            };

            if (salesTax?.ExciseFees?.Count > 0)
            {
                foreach (var fee in salesTax.ExciseFees)
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

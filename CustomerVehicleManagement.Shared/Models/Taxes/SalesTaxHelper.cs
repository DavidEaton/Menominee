using CustomerVehicleManagement.Domain.Entities.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class SalesTaxHelper
    {
        public static SalesTax ConvertWriteDtoToEntity(SalesTaxToWrite tax)
        {
            return tax is null
                ? null
                : SalesTax.Create(
                tax.Description,
                tax.TaxType,
                tax.Order,
                tax.TaxIdNumber,
                tax.PartTaxRate,
                tax.LaborTaxRate,
                ExciseFeeHelper.ConvertWriteDtosToEntities(tax.ExciseFees),
                tax.IsAppliedByDefault,
                tax.IsTaxable)
                .Value;
        }

        public static SalesTaxToWrite CovertReadToWriteDto(SalesTaxToRead taxToRead)
        {
            return taxToRead is null
                ? null
                : new()
                {
                    Description = taxToRead.Description,
                    TaxType = taxToRead.TaxType,
                    Order = taxToRead.Order,
                    IsAppliedByDefault = taxToRead.IsAppliedByDefault,
                    IsTaxable = taxToRead.IsTaxable,
                    TaxIdNumber = taxToRead.TaxIdNumber,
                    PartTaxRate = taxToRead.PartTaxRate,
                    LaborTaxRate = taxToRead.LaborTaxRate,
                    ExciseFees = ExciseFeeHelper.CovertReadToWriteDtos(taxToRead?.ExciseFees)
                };
        }

        public static SalesTaxToRead ConvertEntityToReadDto(SalesTax salesTax)
        {
            return salesTax is null
                ? null
                : new()
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
                    ExciseFees = ExciseFeeHelper.ConvertEntitiesToReadDtos(salesTax?.ExciseFees)
                };
        }

        public static SalesTaxToReadInList ConvertEntityToReadInListDto(SalesTax tax)
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

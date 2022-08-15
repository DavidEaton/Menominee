using CustomerVehicleManagement.Domain.Entities.Taxes;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class ExciseFeeHelper
    {
        public static ExciseFee CreateExciseFee(ExciseFeeToWrite exciseFee)
        {
            return exciseFee is null
                ? null
                : ExciseFee.Create(exciseFee.Description, exciseFee.FeeType, exciseFee.Amount)
                .Value;
        }

        public static ExciseFeeToWrite CreateExciseFee(ExciseFeeToRead exciseFee)
        {
            return exciseFee is null
                ? null
                : new()
                {
                    Description = exciseFee.Description,
                    FeeType = exciseFee.FeeType,
                    Amount = exciseFee.Amount
                };
        }

        public static ExciseFeeToRead CreateExciseFee(ExciseFee exciseFee)
        {
            return exciseFee is null
                ? null
                : new()
                {
                    Id = exciseFee.Id,
                    Description = exciseFee.Description,
                    FeeType = exciseFee.FeeType,
                    Amount = exciseFee.Amount
                };
        }

        public static ExciseFeeToReadInList CreateExciseFeeInList(ExciseFee excisefee)
        {
            return excisefee is null
                ? null
                : new()
                {
                    Id = excisefee.Id,
                    Description = excisefee.Description,
                    FeeType = excisefee.FeeType,
                    Amount = excisefee.Amount
                };
        }

        public static List<SalesTaxTaxableExciseFeeToRead> ConvertEntitiesToReadDtos(List<SalesTaxTaxableExciseFee> excisefees)
        {
            return
                excisefees?.Select(fee =>
                new SalesTaxTaxableExciseFeeToRead()
                {
                    Id = fee.Id,
                    ExciseFee = CreateExciseFee(fee.ExciseFee),
                    SalesTax = SalesTaxHelper.CreateSalesTax(fee.SalesTax)
                }).ToList()
            ??
                 new List<SalesTaxTaxableExciseFeeToRead>();
        }
    }
}

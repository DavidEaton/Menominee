using CustomerVehicleManagement.Domain.Entities.Taxes;
using System;
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

        public static List<ExciseFeeToRead> CreateExciseFees(List<ExciseFee> excisefees)
        {
            return
                excisefees?.Select(fee =>
                new ExciseFeeToRead()
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                }).ToList()
                ??
                new List<ExciseFeeToRead>();
        }

        public static List<ExciseFee> CreateExciseFees(List<ExciseFeeToWrite> exciseFees)
        {
            return
                exciseFees?.Select(fee => CreateExciseFee(fee)).ToList()
                ??
                new List<ExciseFee>();
        }
    }
}

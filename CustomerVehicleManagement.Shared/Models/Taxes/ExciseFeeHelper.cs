using CustomerVehicleManagement.Domain.Entities.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class ExciseFeeHelper
    {
        public static ExciseFee ConvertWriteDtoToEntity(ExciseFeeToUpdate exciseFee)
        {
            return exciseFee is null
                ? null
                : ExciseFee.Create(
                    exciseFee.Description,
                    exciseFee.FeeType,
                    exciseFee.Amount)
                .Value;
        }

        public static ExciseFeeToUpdate CovertReadToWriteDto(ExciseFeeToRead exciseFee)
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

        public static ExciseFeeToRead ConvertEntityToReadDto(ExciseFee exciseFee)
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

        public static ExciseFeeToReadInList ConvertEntityToReadInListDto(ExciseFee excisefee)
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

        internal static List<ExciseFeeToUpdate> CovertReadToWriteDtos(List<ExciseFeeToRead> exciseFees)
        {
            return
                exciseFees?.Select(fee =>
                new ExciseFeeToUpdate()
                {
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                }).ToList()
                ??
                new List<ExciseFeeToUpdate>();
        }

        public static List<ExciseFeeToRead> ConvertEntitiesToReadDtos(IReadOnlyList<ExciseFee> exciseFees)
        {
            return
                exciseFees?.Select(fee =>
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

        public static List<ExciseFee> ConvertWriteDtosToEntities(List<ExciseFeeToUpdate> exciseFees)
        {
            return
                exciseFees?.Select(fee => ConvertWriteDtoToEntity(fee)).ToList()
                ??
                new List<ExciseFee>();
        }

        public static ExciseFee ConvertAddDtoToEntity(ExciseFeeToAdd excisefee)
        {
            return excisefee is null
                ? null
                : ExciseFee.Create(
                    excisefee.Description,
                    excisefee.FeeType,
                    excisefee.Amount)
                .Value;
        }

        internal static List<ExciseFee> CovertReadDtoToEntity(List<ExciseFeeToRead> exciseFees)
        {
            return exciseFees.Select(fee =>
                    ExciseFee.Create(
                        fee.Description,
                        fee.FeeType,
                        fee.Amount)
                    .Value)
                .ToList();
        }
    }
}

using Menominee.Domain.Entities.Taxes;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Taxes
{
    public class ExciseFeeHelper
    {
        public static ExciseFee ConvertWriteDtoToEntity(ExciseFeeToWrite exciseFee)
        {
            return exciseFee is null
                ? null
                : ExciseFee.Create(
                    exciseFee.Description,
                    exciseFee.FeeType,
                    exciseFee.Amount)
                .Value;
        }

        public static ExciseFeeToWrite CovertReadToWriteDto(ExciseFeeToRead exciseFee)
        {
            return exciseFee is null
                ? new()
                : new()
                {
                    Description = exciseFee.Description,
                    FeeType = exciseFee.FeeType,
                    Amount = exciseFee.Amount
                };
        }

        public static ExciseFeeToRead ConvertToReadDto(ExciseFee exciseFee)
        {
            return exciseFee is null
                ? new()
                : new()
                {
                    Id = exciseFee.Id,
                    Description = exciseFee.Description,
                    FeeType = exciseFee.FeeType,
                    Amount = exciseFee.Amount
                };
        }

        public static ExciseFeeToReadInList ConvertToReadInListDto(ExciseFee excisefee)
        {
            return excisefee is null
                ? new()
                : new()
                {
                    Id = excisefee.Id,
                    Description = excisefee.Description,
                    FeeType = excisefee.FeeType,
                    Amount = excisefee.Amount
                };
        }

        internal static List<ExciseFeeToWrite> CovertReadToWriteDtos(List<ExciseFeeToRead> exciseFees)
        {
            return exciseFees?.Select(
                fee =>
                new ExciseFeeToWrite()
                {
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                }).ToList()
            ?? new List<ExciseFeeToWrite>();
        }

        public static List<ExciseFeeToRead> ConvertToReadDtos(IReadOnlyList<ExciseFee> exciseFees)
        {
            return exciseFees?.Select(
                fee =>
                new ExciseFeeToRead()
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                }).ToList()
            ?? new List<ExciseFeeToRead>();
        }

        public static List<ExciseFee> ConvertWriteDtosToEntities(List<ExciseFeeToWrite> exciseFees)
        {
            return exciseFees?.Select(
                fee =>
                ExciseFee.Create(
                    fee.Description,
                    fee.FeeType,
                    fee.Amount)
                .Value).ToList()
            ?? new List<ExciseFee>();
        }

        internal static List<ExciseFee> CovertReadDtoToEntity(List<ExciseFeeToRead> exciseFees)
        {
            return exciseFees.Select(fee =>
                    ExciseFee.Create(
                        fee.Description,
                        fee.FeeType,
                        fee.Amount)
                    .Value)
                .ToList()
                ?? new List<ExciseFee>();
        }

        internal static List<ExciseFeeToWrite> ConvertToWriteDtos(IReadOnlyList<ExciseFee> exciseFees)
        {
            return exciseFees?.Select(
                fee =>
                new ExciseFeeToWrite()
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                }).ToList()
            ?? new List<ExciseFeeToWrite>();
        }
    }
}

using Menominee.Domain.Entities.RepairOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.SerialNumbers
{
    public class SerialNumberHelper
    {

        public static List<RepairOrderSerialNumberToWrite> CovertReadToWriteDtos(IReadOnlyList<RepairOrderSerialNumberToRead> serialNumbers)
        {
            return serialNumbers?.Select(
                serialNumber =>
                new RepairOrderSerialNumberToWrite()
                {
                    Id = serialNumber.Id,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList()
            ?? new List<RepairOrderSerialNumberToWrite>();
        }

        public static List<RepairOrderSerialNumberToWrite> ConvertWriteToReadDtos(IReadOnlyList<RepairOrderSerialNumberToRead> serialNumbers)
        {
            return serialNumbers?.Select(
                serialNumber =>
                new RepairOrderSerialNumberToWrite()
                {
                    Id = serialNumber.Id,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList()
            ?? new List<RepairOrderSerialNumberToWrite>();
        }

        public static IReadOnlyList<RepairOrderSerialNumberToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderSerialNumber> serialNumbers)
        {
            return serialNumbers?.Select(
                serialNumber =>
                new RepairOrderSerialNumberToRead()
                {
                    Id = serialNumber.Id,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList()
            ?? new List<RepairOrderSerialNumberToRead>();
        }

        internal static List<RepairOrderSerialNumberToWrite> CovertToWriteDtos(IReadOnlyList<RepairOrderSerialNumber> serialNumbers)
        {
            return serialNumbers?.Select(
                serialNumber =>
                new RepairOrderSerialNumberToWrite()
                {
                    Id = serialNumber.Id,
                    SerialNumber = serialNumber.SerialNumber
                }).ToList()
            ?? new List<RepairOrderSerialNumberToWrite>();
        }
    }
}

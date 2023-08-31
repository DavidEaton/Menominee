using Menominee.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Statuses
{
    public class StatusHelper
    {
        public static List<RepairOrderStatusToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderStatus> statuses)
        {
            return statuses?.Select(
                status =>
                new RepairOrderStatusToRead
                {
                    Id = status.Id,
                    Status = status.Type,
                    Description = status.Description,
                    Date = status.Date
                }).ToList()
            ?? new List<RepairOrderStatusToRead>();
        }

        public static List<RepairOrderStatusToWrite> CovertReadToWriteDtos(List<RepairOrderStatusToRead> statuses)
        {
            return statuses?.Select(
                status =>
                new RepairOrderStatusToWrite()
                {
                    Id = status.Id,
                    Status = status.Status,
                    Description = status.Description,
                    Date = status.Date,
                }).ToList()
            ?? new List<RepairOrderStatusToWrite>();
        }

        public static List<RepairOrderStatus> ConvertWriteDtosToEntities(List<RepairOrderStatusToWrite> statuses)
        {
            return statuses?.Select(
                status =>
                RepairOrderStatus.Create(
                status.Status,
                status.Description
                ).Value)
            .ToList()
            ?? new List<RepairOrderStatus>();
        }

        public static List<RepairOrderStatusToWrite> ConvertToWriteDtos(List<RepairOrderStatus> statuses)
        {
            return statuses?.Select(
                status =>
                new RepairOrderStatusToWrite()
                {
                    Id = status.Id,
                    Status = status.Type,
                    Description = status.Description,
                    Date = status.Date,
                }).ToList()
            ?? new List<RepairOrderStatusToWrite>();
        }
    }
}

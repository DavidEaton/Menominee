using Menominee.Domain.Entities.RepairOrders;
using System;

namespace Menominee.Shared.Models.RepairOrders.Statuses
{
    public class RepairOrderStatusToWrite
    {
        public long Id { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}

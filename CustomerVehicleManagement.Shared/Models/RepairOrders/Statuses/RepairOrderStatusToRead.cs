﻿using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Statuses
{
    public class RepairOrderStatusToRead
    {
        public long Id { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
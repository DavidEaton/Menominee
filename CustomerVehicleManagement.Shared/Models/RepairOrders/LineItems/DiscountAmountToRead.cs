﻿using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems
{
    public class DiscountAmountToRead
    {
        public ItemDiscountType DiscountType { get; set; }
        public double Amount { get; set; }
    }
}
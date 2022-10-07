﻿using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class LaborAmountToRead
    {
        public long Id { get; set; }
        public ItemLaborType PayType { get; set; }
        public double Amount { get; set; }
    }
}

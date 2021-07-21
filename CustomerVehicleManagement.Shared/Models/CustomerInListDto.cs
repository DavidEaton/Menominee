﻿using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerInListDto
    {
        public int Id { get; set; }
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string AddressFull { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public string CustomerType { get; set; }
        public string Note { get; set; }
        public string ContactName { get; set; }
        public string ContactPrimaryPhone { get; set; }
        public string ContactPrimaryPhoneType { get; set; }

    }
}
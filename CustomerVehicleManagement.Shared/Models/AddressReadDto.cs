﻿using SharedKernel.Enums;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressReadDto
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }

        public static AddressReadDto ConvertToDto(Address address)
        {
            if (address != null)
            {
                var addressReadDto = new AddressReadDto
                {
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                };

                return addressReadDto;
            }
            return null;
        }
    }
}

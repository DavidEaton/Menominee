using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneToRead
    {
        public string Number { get; set; }
        public string PhoneType { get; set; }
        public bool IsPrimary { get; set; }


        public static IReadOnlyList<PhoneToRead> ConvertToDto(IList<Phone> phones)
        {
            return phones
                .Select(phone =>
                        ConvertToDto(phone))
                .ToList();
        }

        private static PhoneToRead ConvertToDto(Phone phone)
        {
            if (phone != null)
            {
                return new PhoneToRead()
                {
                    Number = phone.Number,
                    PhoneType = phone.PhoneType.ToString(),
                    IsPrimary = phone.IsPrimary
                };
            }

            return null;
        }
    }
}

using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Utilities
{
    public static class PhoneHelpers
    {
        public static bool PrimaryPhoneExists(IList<Phone> phones)
        {
            if (phones == null)
                return false;

            bool result = false;

            foreach (var existingPhone in phones)
            {
                if (existingPhone.IsPrimary)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool DuplicatePhoneNumberExists(Phone phone, IList<Phone> phones)
        {
            if (phones == null)
                return false;

            bool result = false;

            foreach (var existingPhone in phones)
            {
                if (existingPhone.Number == phone.Number)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}

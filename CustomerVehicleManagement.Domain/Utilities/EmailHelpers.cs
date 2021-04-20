using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Domain.Utilities
{
    public static class EmailHelpers
    {
        public static bool PrimaryEmailExists(IList<Email> emails)
        {
            if (emails == null)
                return false;

            bool result = false;

            foreach (var existingEmail in emails)
            {
                if (existingEmail.IsPrimary)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool PrimaryEmailCountExceedsOne(IList<Email> emails)
        {
            if (emails == null)
                return false;

            int primaryEmailCount = 0;

            foreach (var existingEmail in emails)
            {
                if (existingEmail.IsPrimary)
                {
                    primaryEmailCount += 1;
                }
            }

            return primaryEmailCount > 1;
        }

        public static bool DuplicateEmailExists(IList<Email> emails)
        {
            if (emails == null)
                return false;

            return emails.Count != emails.Distinct().Count();
        }

        public static bool DuplicateEmailExists(Email email, IList<Email> emails)
        {
            if (emails == null)
                return false;

            bool result = false;

            foreach (var existingEmail in emails)
            {
                if (existingEmail.Address == email.Address)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

    }
}

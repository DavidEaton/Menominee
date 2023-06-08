using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.Persons.PersonNames
{
    public class PersonNameHelper
    {
        public static PersonNameToRead ConvertToReadDto(PersonName name)
        {
            return name is not null
                ? new PersonNameToRead()
                {
                    FirstName = name.FirstName,
                    MiddleName = name.MiddleName,
                    LastName = name.LastName
                }
                : null;
        }
    }
}

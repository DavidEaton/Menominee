using CustomerVehicleManagement.Domain.Enums;
using SharedKernel;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Phone : Entity
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";

        // Blazor 5 requires public JsonConstructor-attributed contructor, 
        [JsonConstructor]
        public Phone(string number, PhoneType phoneType)
        {
            try
            {
                Guard.ForNullOrEmpty(number, "number");

            }
            catch (Exception)
            {
                throw new ArgumentException(PhoneEmptyMessage);
            }

            Number = number;
            PhoneType = phoneType;
        }

        public string Number { get; }
        public PhoneType PhoneType { get; }

        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType);
        }

        public Phone NewPhoneType(PhoneType newPhoneType)
        {
            return new Phone(Number, newPhoneType);
        }

        #region ORM

        // EF requires an empty constructor
        protected Phone() { }

        #endregion

    }

}

using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Phone : Entity
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";

        // Blazor 5 requires public JsonConstructor-attributed contructor, 
        [JsonConstructor]
        public Phone(string number, PhoneType phoneType, bool primary)
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
            Primary = primary;
        }

        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set;}
        public bool Primary { get; private set; }

        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType, Primary);
        }

        public Phone NewPhoneType(PhoneType newPhoneType)
        {
            return new Phone(Number, newPhoneType, Primary);
        }
        public Phone NewPrimary(bool primary)
        {
            return new Phone(Number, PhoneType, primary);
        }

        public override string ToString()
        {
            Number = RemoveNonNumericCharacters(Number);

            switch (Number.Length)
            {
                case 7:
                    return Regex.Replace(Number, @"(\d{3})(\d{4})", "$1-$2");

                case 10:
                    return Regex.Replace(Number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                default:
                    return Number;
            }
        }

        /// <summary>
        /// EqualsByProperty compares each member property and returns true of they are all equal between phone1 and phone2.
        /// </summary>
        /// <param name="phone1"></param>
        /// <param name="phone2"></param>
        /// <returns>True if all properties are equal between phone1 and phone2; False if one or more properties are not equal between phone1 and phone2.</returns>
        public static bool EqualsByProperty(Phone phone1, Phone phone2)
        {
            return phone1.Number == phone2.Number
                && phone1.PhoneType == phone2.PhoneType
                && phone1.Primary == phone2.Primary;
        }

        private static string RemoveNonNumericCharacters(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        #region ORM

        // EF requires an empty constructor
        protected Phone() { }

        #endregion

    }

}

﻿using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
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

        public string Number { get; }
        public PhoneType PhoneType { get; }
        public bool Primary { get; }

        public Phone NewNumber(string newNumber)
        {
            return new Phone(newNumber, PhoneType, Primary);
        }

        public Phone NewPhoneType(PhoneType newPhoneType)
        {
            return new Phone(Number, newPhoneType, Primary);
        }
        public Phone NewPrimary(string number)
        {
            return new Phone(number, PhoneType, true);
        }

        public override string ToString()
        {
            switch (Number.Length)
            {
                case 7:
                    return Regex.Replace(Number, @"(\d{3})(\d{4})", "$2-$3");

                case 10:
                    return Regex.Replace(Number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                default:
                    return Number;
            }
        }

        #region ORM

        // EF requires an empty constructor
        protected Phone() { }

        #endregion

    }

}
using CustomerVehicleManagement.Domain.Interfaces;
using CustomerVehicleManagement.Domain.Utilities;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Person : Entity, IListOfPhone
    {
        public static readonly string DuplicatePhoneExistsMessage = "Cannot add duplicate phone.";
        public static readonly string PrimaryPhoneExistsMessage = "Cannot add more than one Primary phone.";
        public static readonly string DuplicateEmailExistsMessage = "Cannot add duplicate email.";
        public static readonly string PrimaryEmailExistsMessage = "Cannot add more than one Primary email.";
        public static readonly string EmptyEmailCollectionMessage = "Cannot add an empty email list";

        public Person(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday, Address address, IList<Phone> phones)
            : this(name, gender, birthday, address, phones, null) { }

        // Blazor 5 requires public JsonConstructor-attributed contructor
        // HOW IS THE PRESENTATION LAYER "REQUIRING" ANYTHING OF OUR DOIMAIN MODEL???? FIX THIS!
        // Postman can succesfully request Persons from this endpoint with [JsonConstructor] removed.
        //[JsonConstructor]
        public Person(PersonName name, Gender gender, DateTime? birthday, Address address, IList<Phone> phones, DriversLicense driversLicense = null)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = address;
            DriversLicense = driversLicense;
            Phones = phones;
        }

        public PersonName Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DriversLicense DriversLicense { get; private set; }
        public Address Address { get; private set; }
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();

        public void AddPhone(Phone phone)
        {
            if (PhoneHelpers.DuplicatePhoneNumberExists(phone, Phones))
                throw new ArgumentException(DuplicatePhoneExistsMessage);

            if (PhoneHelpers.PrimaryPhoneExists(Phones) && phone.IsPrimary)
                throw new ArgumentException(PrimaryPhoneExistsMessage);

            if (Phones == null)
                Phones = new List<Phone>();

            if (Phones != null)
                Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Phones.Remove(phone);
        }

        public void SetPhones(IList<Phone> phones)
        {
            if (phones != null)
                Phones = phones;
        }
        public void AddEmail(Email email)
        {
            if (EmailHelpers.DuplicateEmailExists(email, Emails))
                throw new ArgumentException(DuplicateEmailExistsMessage);

            if (EmailHelpers.PrimaryEmailExists(Emails) && email.IsPrimary)
                throw new ArgumentException(PrimaryEmailExistsMessage);

            if (Emails == null)
                Emails = new List<Email>();

            if (Emails != null)
                Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            if (emails == null)
                throw new ArgumentException(EmptyEmailCollectionMessage);


            if (EmailHelpers.DuplicateEmailExists(emails))
                throw new ArgumentException(DuplicateEmailExistsMessage);


            if (EmailHelpers.PrimaryEmailCountExceedsOne(emails))
                throw new ArgumentException(PrimaryEmailExistsMessage);

            Emails = emails;
        }

        public void SetName(PersonName name)
        {
            Name = name;
        }

        public void SetGender(Gender gender)
        {
            Gender = gender;
        }

        public void SetBirthday(DateTime? birthday)
        {
            if (birthday.HasValue)
                Birthday = birthday;
        }

        public void SetDriversLicense(DriversLicense driversLicense)
        {
            DriversLicense = driversLicense;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        #region ORM
        // Code that pollutes our domain class (very minor impact in this case), but
        // is necessary for EntityFramework, makes our model <100% persistence ignorant.

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}

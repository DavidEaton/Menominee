﻿using Bogus;
using CustomerVehicleManagement.Api.Common;
using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using TestingHelperLibrary.Payables;
using Xunit;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class VendorShould
    {
        private static readonly Faker Faker = new Faker();

        [Fact]
        public void Create_Vendor()
        {
            // Arrange
            var name = "Vendor One";
            var vendorCode = "V1";

            // Act
            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            // Assert
            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.Name.Should().Be(name);
            vendorOrError.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Optional_DefaultPaymentMethod()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, string.Empty, defaultPaymentMethod);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.DefaultPaymentMethod.Should().BeOfType<DefaultPaymentMethod>();
            vendorOrError.Value.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);
            vendorOrError.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Null_DefaultPaymentMethod()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            DefaultPaymentMethod defaultPaymentMethod = null;

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, string.Empty, defaultPaymentMethod);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.DefaultPaymentMethod.Should().Be(null);
            vendorOrError.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Optional_Emails()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone0 = Phone.Create(number, phoneType, true).Value;
            phones.Add(phone0);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            var phone1 = Phone.Create(number, phoneType, false).Value;
            phones.Add(phone1);

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Optional_Phones()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var phones = ContactableTestHelper.CreatePhones(10);

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, phones: phones);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.Phones.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public void Not_Create_Vendor_With_Inavlid_VendorRole()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendorRole = (VendorRole)(-1);

            var vendorOrError = Vendor.Create(name, vendorCode, vendorRole);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Create_Vendor_With_Truncated_Note()
        {
            var name = RandomCharacters(Vendor.MinimumLength);
            var vendorNote = RandomCharacters(Contactable.NoteMaximumLength * 2);
            var vendorCode = RandomCharacters(Vendor.MinimumLength);

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, vendorNote);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Notes.Length.Should().Be(Contactable.NoteMaximumLength);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Vendor_With_Invalid_Name(int length)
        {
            var name = RandomCharacters(length);
            var vendorCode = RandomCharacters(Vendor.MinimumLength);

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Vendor_With_Invalid_Code(int length)
        {
            var name = RandomCharacters(Vendor.MinimumLength);
            var vendorCode = RandomCharacters(length);

            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var vendor = VendorTestHelper.CreateVendor();

            var name = RandomCharacters(Vendor.MinimumLength + 11);
            vendor.SetName(name);

            vendor.Name.Should().Be(name);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Name_With_Invalid_Name(int length)
        {
            var name = RandomCharacters(Vendor.MinimumLength);
            var vendorCode = RandomCharacters(Vendor.MinimumLength);
            var vendorOrError = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            var invalidName = RandomCharacters(length);
            var resultOrError = vendorOrError.Value.SetName(invalidName);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetVendorCode()
        {
            var vendor = VendorTestHelper.CreateVendor();
            vendor.VendorCode.Length.Should().Be(Vendor.MinimumLength);
            var vendorCode = RandomCharacters(Vendor.MinimumLength + 1);

            vendor.SetVendorCode(vendorCode);

            vendor.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void SetNote()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var vendorNote = RandomCharacters(Contactable.NoteMaximumLength);
            vendor.SetNotes(vendorNote);

            vendor.Notes.Should().Be(vendorNote);
        }

        [Fact]
        public void Set_Truncated_Note()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var vendorNote = RandomCharacters(Contactable.NoteMaximumLength * 2);
            vendor.SetNotes(vendorNote);

            vendor.Notes.Length.Should().Be(Contactable.NoteMaximumLength);
        }

        [Fact]
        public void SetDefaultPaymentMethod()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;

            vendor.SetDefaultPaymentMethod(defaultPaymentMethod);

            vendor.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);
        }

        [Fact]
        public void Not_SetDefaultPaymentMethod_With_Null_DefaultPaymentMethod()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            DefaultPaymentMethod defaultPaymentMethod = null;

            var resultOrError = vendor.SetDefaultPaymentMethod(defaultPaymentMethod);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void ClearDefaultPaymentMethod()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            vendor.SetDefaultPaymentMethod(defaultPaymentMethod);
            vendor.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);

            var resultOrError = vendor.ClearDefaultPaymentMethod();

            resultOrError.IsFailure.Should().BeFalse();
            vendor.DefaultPaymentMethod.Should().Be(null);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Vendor_Code(int length)
        {
            var vendor = VendorTestHelper.CreateVendor();

            var invalidVendorCode = RandomCharacters(length);
            var resultOrError = vendor.SetVendorCode(invalidVendorCode);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Disable()
        {
            var vendor = VendorTestHelper.CreateVendor();

            vendor.Disable();

            vendor.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Enable()
        {
            var vendor = VendorTestHelper.CreateVendor();

            vendor.Disable();
            vendor.IsActive.Should().BeFalse();

            vendor.Enable();
            vendor.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_Duplicate_Phone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phoneOne = Phone.Create(number, phoneType, true).Value;
            var numberTwo = "555.444.3333";
            var phoneTypeTwo = PhoneType.Work;
            var phoneTwo = Phone.Create(numberTwo, phoneTypeTwo, true).Value;

            vendor.AddPhone(phoneOne);
            var vendorOrError = vendor.AddPhone(phoneTwo);

            vendor.Phones.Should().Contain(phoneOne);
            vendor.Phones.Should().NotContain(phoneTwo);
            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        public void Not_Add_Invalid_Phone(Phone phone)
        {
            var vendor = VendorTestHelper.CreateVendor();

            var result = vendor.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            vendor.Phones.Count.Should().Be(0);

            vendor.AddPhone(phone);
            vendor.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void AddPhone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            vendor.AddPhone(phone);

            vendor.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            vendor.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            vendor.AddPhone(phone);

            vendor.Phones.Count.Should().Be(2);
            vendor.RemovePhone(phone);

            vendor.Phones.Count.Should().Be(1);

        }

        [Fact]
        public void Not_Add_More_Than_One_Primary_Phone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "555.627.9206";
            var phone = Phone.Create(number, PhoneType.Home, true).Value;
            vendor.AddPhone(phone);
            number = "444.627.9206";
            phone = Phone.Create(number, PhoneType.Mobile, true).Value;

            var result = vendor.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Have_Empty_Emails_On_Create()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            vendor.Emails.Count.Should().Be(0);

            vendor.AddEmail(email);
            vendor.Emails.Count.Should().Be(1);
        }
        [Fact]

        public void AddEmail()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            vendor.AddEmail(email);

            vendor.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            vendor.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            vendor.AddEmail(email1);

            vendor.Emails.Count.Should().Be(2);
            vendor.RemoveEmail(email0);

            vendor.Emails.Count.Should().Be(1);
            vendor.Emails.Should().Contain(email1);
        }

        [Fact]
        public void Not_Add_More_Than_One_Primary_Email()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            vendor.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            var result = vendor.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_Duplicate_Email()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            vendor.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = vendor.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        public void Not_Add_Invalid_Email(Email email)
        {
            var vendor = VendorTestHelper.CreateVendor();

            var result = vendor.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void ClearAddress()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = Address.Create(
                "1234 Five Street",
                "Gaylord",
                State.MI,
                "49735").Value;
            vendor.SetAddress(address);
            vendor.Address.Should().Be(address);

            vendor.ClearAddress();

            vendor.Address.Should().BeNull();
        }

        [Fact]
        public void SetAddress()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = Address.Create(
                "1234 Five Street",
                "Gaylord",
                State.MI,
                "49735").Value;

            vendor.SetAddress(address);
            vendor.Address.Should().Be(address);
        }

        [Fact]
        public void SetVendorRole()
        {
            var vendor = VendorTestHelper.CreateVendor();
            vendor.VendorRole.Should().Be(VendorRole.PartsSupplier);
            var newVendorRole = VendorRole.PaymentReconciler;

            vendor.SetVendorRole(newVendorRole);

            vendor.VendorRole.Should().Be(newVendorRole);
        }


        [Fact]
        public void Not_Set_Inavlid_VendorRole()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var invalidVendorRole = (VendorRole)(-1);

            var vendorOrError = vendor.SetVendorRole(invalidVendorRole);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SyncContactDetails()
        {
            var vendor = new VendorFaker(true, emailsCount: 3, phonesCount: 3).Generate();

            var updatedAddress = new AddressFaker().Generate();

            var updatedPhones = vendor.Phones.Select(phone =>
            {
                return new PhoneToWrite
                {
                    Id = phone.Id,
                    PhoneType = phone.PhoneType,
                    Number = Faker.Phone.PhoneNumber("##########"),
                    IsPrimary = phone.IsPrimary
                };
            }).ToList();

            var updatedEmails = vendor.Emails.Select(email => new EmailToWrite
            {
                Id = email.Id,
                Address = $"updated-{email.Address}",
                IsPrimary = email.IsPrimary
            }).ToList();

            var updatedContactDetails = ContactDetailsFactory.Create(
                phonesToWrite: updatedPhones,
                emailsToWrite: updatedEmails,
                addressToWrite: AddressHelper.ConvertToWriteDto(updatedAddress));

            vendor.Phones.Should().NotBeEquivalentTo(updatedPhones);
            vendor.Emails.Should().NotBeEquivalentTo(updatedEmails);
            vendor.Address.Should().NotBe(updatedAddress);

            vendor.SyncContactDetails(updatedContactDetails);

            vendor.Phones.Should().BeEquivalentTo(updatedPhones);
            vendor.Emails.Should().BeEquivalentTo(updatedEmails);
            vendor.Address.Should().Be(updatedAddress);
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { null };
                    yield return new object[] { Vendor.MinimumLength - 1 };
                    yield return new object[] { Vendor.MaximumLength + 1 };
                }
            }

            public static IEnumerable<object[]> NoteData
            {
                get
                {
                    yield return new object[] { Vendor.MinimumLength - 1 };
                    yield return new object[] { Vendor.NoteMaximumLength + 1 };
                }
            }

        }
    }
}

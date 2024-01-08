using Bogus;
using FluentAssertions;
using Menominee.Api.Features.Contactables;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Enums;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.TestingHelperLibrary.Fakers;
using System.Collections.Generic;
using System.Linq;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using TestingHelperLibrary.Payables;
using Xunit;
using static Menominee.Tests.Utilities;

namespace Menominee.Tests.Entities
{
    public class VendorShould
    {
        private static readonly Faker Faker = new();

        [Fact]
        public void Create_Vendor()
        {
            // Arrange
            var name = "Vendor One";
            var vendorCode = "V1";

            // Act
            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<Vendor>();
            result.Value.Name.Should().Be(name);
            result.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Optional_DefaultPaymentMethod()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, string.Empty, defaultPaymentMethod);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<Vendor>();
            result.Value.DefaultPaymentMethod.Should().BeOfType<DefaultPaymentMethod>();
            result.Value.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);
            result.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Null_DefaultPaymentMethod()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            DefaultPaymentMethod defaultPaymentMethod = null;

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, string.Empty, defaultPaymentMethod);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<Vendor>();
            result.Value.DefaultPaymentMethod.Should().Be(null);
            result.Value.VendorCode.Should().Be(vendorCode);
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

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<Vendor>();
            result.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Optional_Phones()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var phones = ContactableTestHelper.CreatePhones(10);

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, phones: phones);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<Vendor>();
            result.Value.Phones.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public void Return_Failure_On_Create_Vendor_With_Inavlid_VendorRole()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendorRole = (VendorRole)(-1);

            var result = Vendor.Create(name, vendorCode, vendorRole);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Create_Vendor_With_Truncated_Note()
        {
            var name = RandomCharacters(Vendor.MinimumLength);
            var vendorNote = RandomCharacters(Contactable.NoteMaximumLength * 2);
            var vendorCode = RandomCharacters(Vendor.MinimumLength);

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier, vendorNote);

            result.IsFailure.Should().BeFalse();
            result.Value.Notes.Length.Should().Be(Contactable.NoteMaximumLength);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_Vendor_With_Invalid_Name(int length)
        {
            var name = RandomCharacters(length);
            var vendorCode = RandomCharacters(Vendor.MinimumLength);

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_Vendor_With_Invalid_Code(int length)
        {
            var name = RandomCharacters(Vendor.MinimumLength);
            var vendorCode = RandomCharacters(length);

            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            result.IsFailure.Should().BeTrue();
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
        public void Return_Failure_On_Set_Name_With_Invalid_Name(int length)
        {
            var name = RandomCharacters(Vendor.MinimumLength);
            var vendorCode = RandomCharacters(Vendor.MinimumLength);
            var result = Vendor.Create(name, vendorCode, VendorRole.PartsSupplier);

            var invalidName = RandomCharacters(length);
            var resultOrError = result.Value.SetName(invalidName);

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
        public void Return_Failure_On_SetDefaultPaymentMethod_With_Null_DefaultPaymentMethod()
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
        public void Return_Failure_On_Set_Invalid_Vendor_Code(int length)
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
        public void Return_Failure_On_Add_Duplicate_Phone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phoneOne = Phone.Create(number, phoneType, true).Value;
            var numberTwo = "555.444.3333";
            var phoneTypeTwo = PhoneType.Work;
            var phoneTwo = Phone.Create(numberTwo, phoneTypeTwo, true).Value;

            vendor.AddPhone(phoneOne);
            var result = vendor.AddPhone(phoneTwo);

            vendor.Phones.Should().Contain(phoneOne);
            vendor.Phones.Should().NotContain(phoneTwo);
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        public void Return_Failure_On_Add_Invalid_Phone(Phone phone)
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
        public void Return_Failure_On_Remove_Null_Phone()
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
            var result = vendor.RemovePhone(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
            vendor.Phones.Count.Should().Be(2);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Phone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            vendor.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;

            vendor.Phones.Count.Should().Be(1);
            var result = vendor.RemovePhone(phone);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.NotFoundMessage);
            vendor.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Phone()
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
        public void Return_Failure_On_Remove_Null_Email()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            vendor.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            vendor.AddEmail(email1);

            vendor.Emails.Count.Should().Be(2);
            var result = vendor.RemoveEmail(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
            vendor.Emails.Count.Should().Be(2);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Email()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            vendor.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;

            vendor.Emails.Count.Should().Be(1);
            var result = vendor.RemoveEmail(email1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.NotFoundMessage);
            vendor.Emails.Count.Should().Be(1);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Email()
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
        public void Return_Failure_On_Add_Duplicate_Email()
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
        public void Return_Failure_On_Add_Invalid_Email(Email email)
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
        public void Return_Failure_On_Set_Inavlid_Address()
        {
            var vendor = VendorTestHelper.CreateVendor();
            Address address = null;

            var result = vendor.SetAddress(address);
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
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
        public void Return_Failure_On_Set_Inavlid_VendorRole()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var invalidVendorRole = (VendorRole)(-1);

            var result = vendor.SetVendorRole(invalidVendorRole);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Update_Added_ContactDetails()
        {
            var generateId = true; // phone/emails created with generateId = true will have Id values.
            var emailsCount = 3;
            var phonesCount = 3;
            var vendor = new VendorFaker(generateId, emailsCount, phonesCount).Generate();
            var addressToAdd = new AddressFaker().Generate();
            var phonesToAdd = PhoneHelper.ConvertToWriteDtos(
                new PhoneFaker(!generateId).Generate(phonesCount));
            var phonesToUpdate =
                vendor.Phones.Select(phone =>
                {
                    return new PhoneToWrite
                    {
                        Id = phone.Id,
                        PhoneType = phone.PhoneType,
                        Number = phone.Number,
                        IsPrimary = phone.IsPrimary
                    };
                }).ToList();
            // Include the exiting phones with phonesToAdd
            phonesToUpdate.AddRange(phonesToAdd);
            var emailsToAdd = EmailHelper.ConvertToWriteDtos(
                new EmailFaker(!generateId).Generate(emailsCount));
            var emailsToUpdate =
                vendor.Emails.Select(email =>
                {
                    return new EmailToWrite
                    {
                        Id = email.Id,
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    };
                }).ToList();
            // Include the exiting emails with emailsToAdd
            emailsToUpdate.AddRange(emailsToAdd);
            vendor.Phones.Should().NotBeEquivalentTo(phonesToUpdate);
            vendor.Emails.Should().NotBeEquivalentTo(emailsToUpdate);
            vendor.Address.Should().NotBe(addressToAdd);
            var updatedContactDetails = ContactDetailsFactory.Create(
                phonesToWrite: phonesToUpdate,
                emailsToWrite: emailsToUpdate,
                addressToWrite: AddressHelper.ConvertToWriteDto(addressToAdd)).Value;

            vendor.UpdateContactDetails(updatedContactDetails);

            vendor.Address.Should().Be(addressToAdd);
            vendor.Phones.Count.Should().Be(phonesCount + phonesCount);
            vendor.Emails.Count.Should().Be(emailsCount + emailsCount);
        }

        [Fact]
        public void Update_Edited_ContactDetails()
        {
            var vendor = new VendorFaker(true, emailsCount: 3, phonesCount: 3, includeAddress: true).Generate();
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
                addressToWrite: AddressHelper.ConvertToWriteDto(updatedAddress)).Value;
            vendor.Phones.Should().NotBeEquivalentTo(updatedPhones);
            vendor.Emails.Should().NotBeEquivalentTo(updatedEmails);
            vendor.Address.Should().NotBe(updatedAddress);

            vendor.UpdateContactDetails(updatedContactDetails);

            vendor.Phones.Should().BeEquivalentTo(updatedPhones);
            vendor.Emails.Should().BeEquivalentTo(updatedEmails);
            vendor.Address.Should().Be(updatedAddress);
        }

        [Fact]
        public void Update_Removed_ContactDetails()
        {
            var generateId = true;
            var emailsCount = 3;
            var phonesCount = 3;
            var vendor = new VendorFaker(generateId, phonesCount: phonesCount, emailsCount: emailsCount, includeAddress: true).Generate();
            vendor.Phones.Count.Should().Be(phonesCount);
            vendor.Emails.Count.Should().Be(emailsCount);
            // remove a phone
            var phoneToRemove = vendor.Phones[0];
            // remove an email
            var emailToRemove = vendor.Emails[0];
            var updatedPhones = vendor.Phones.Select(phone =>
            {
                return new PhoneToWrite
                {
                    Id = phone.Id,
                    PhoneType = phone.PhoneType,
                    Number = phone.Number,
                    IsPrimary = phone.IsPrimary
                };
            }).Where(phone => phone.Id != phoneToRemove.Id)
              .ToList();
            var updatedEmails = vendor.Emails.Select(email =>
            {
                return new EmailToWrite
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                };
            }).Where(email => email.Id != emailToRemove.Id)
              .ToList();
            var updatedContactDetails = ContactDetailsFactory.Create(
                phonesToWrite: updatedPhones,
                emailsToWrite: updatedEmails,
                addressToWrite: AddressHelper.ConvertToWriteDto(vendor.Address)).Value;

            vendor.UpdateContactDetails(updatedContactDetails);

            vendor.Phones.Count.Should().Be(phonesCount - 1);
            vendor.Phones.Should().NotContain(phoneToRemove);
            vendor.Emails.Count.Should().Be(emailsCount - 1);
            vendor.Emails.Should().NotContain(emailToRemove);
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

using CustomerVehicleManagement.Api.Validators;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using FluentValidation.TestHelper;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.ValidatorTests
{
    public class OrganizationToAddValidatorShould
    {
        private readonly OrganizationToAddValidator validator;
        private static readonly Random random = new();
        private static string RandomCharacters(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public OrganizationToAddValidatorShould()
        {
            validator = new OrganizationToAddValidator();
        }

        [Fact]
        public void Have_error_when_Name_is_null()
        {
            var organization = new OrganizationToAdd
            {
                Name = null
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Name)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Name' must not be empty.");
        }

        [Fact]
        public void Have_error_when_Name_is_empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = ""
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Name)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Name' must not be empty.");
        }

        [Fact]
        public void Have_error_when_Name_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = RandomCharacters(256)
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Name)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Name' must be between 2 and 255 characters. You entered 256 characters.");
        }

        [Fact]
        public void Have_error_when_Name_is_too_short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "c"
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Name)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Name' must be between 2 and 255 characters. You entered 1 characters.");
        }

        [Fact]
        public void Have_error_when_Note_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Note = RandomCharacters(10000) + "1"
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Note)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Note' must be between 0 and 10000 characters. You entered 10001 characters.");
        }



        [Fact]
        public void Have_error_when_AddressLine_is_null()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = null,
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.AddressLine)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Address Line' must not be empty.");
        }

        [Fact]
        public void Have_error_when_AddressLine_is_empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.AddressLine)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Address Line' must not be empty.");
        }

        [Fact]
        public void Have_error_when_AddressLine_is_too_short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.AddressLine)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Address Line' must be between 2 and 255 characters. You entered 1 characters.");
        }

        [Fact]
        public void Have_error_when_AddressLine_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = RandomCharacters(256),
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.AddressLine)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Address Line' must be between 2 and 255 characters. You entered 256 characters.");
        }

        [Fact]
        public void Have_error_when_City_is_null()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = null,
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.City)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'City' must not be empty.");
        }

        [Fact]
        public void Have_error_when_City_is_empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = null,
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.City)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'City' must not be empty.");
        }

        [Fact]
        public void Have_error_when_City_is_too_short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "C",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.City)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'City' must be between 2 and 255 characters. You entered 1 characters.");
        }

        [Fact]
        public void Have_error_when_City_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = RandomCharacters(256),
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.City)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'City' must be between 2 and 255 characters. You entered 256 characters.");
        }

        [Fact]
        public void Have_error_when_PostalCode_is_null()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = null
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.PostalCode)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Postal Code' must not be empty.");
        }

        [Fact]
        public void Have_error_when_PostalCode_is_empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = ""
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.PostalCode)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Postal Code' must not be empty.");
        }

        [Fact]
        public void Have_error_when_PostalCode_is_too_short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "4"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.PostalCode)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Postal Code' must be between 5 and 10 characters. You entered 1 characters.");
        }

        [Fact]
        public void Have_error_when_PostalCode_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "12345678910"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.PostalCode)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("'Postal Code' must be between 5 and 10 characters. You entered 11 characters.");
        }

        [Fact]
        public void Have_error_when_PostalCode_is_invalid()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "Z4566"
                }
            };

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Address.PostalCode)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("Must enter a valid Postal Code");
        }

        [Fact]
        public void Have_error_when_Emails_has_more_than_one_Primary()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops"
            };

            organization.Emails.Add(new EmailToAdd
            {
                Address = "a@a.a",
                IsPrimary = true
            });

            organization.Emails.Add(new EmailToAdd
            {
                Address = "b@b.b",
                IsPrimary = true
            });

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Emails)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("Can have only one Primary email.");
        }

        //[Fact]
        //public void Have_error_when_Emails_has_invalid_Address()
        //{
        //    var organization = new OrganizationToAdd
        //    {
        //        Name = "Moops"
        //    };

        //    organization.Emails.Add(new EmailToAdd
        //    {
        //        Address = "aa.a",
        //        IsPrimary = true
        //    });

        //    var result = validator.TestValidate(organization);
        //    result
        //        .ShouldHaveValidationErrorFor(organization => organization.Emails);
        //        //.WithSeverity(Severity.Error)
        //        //.WithErrorMessage("'Address' is not a valid email address.");
        //}

        [Fact]
        public void Have_error_when_Phones_has_more_than_one_Primary()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops"
            };

            organization.Phones.Add(new PhoneToAdd
            {
                Number = "9896279206",
                IsPrimary = true
            });

            organization.Phones.Add(new PhoneToAdd
            {
                Number = "2315462102",
                IsPrimary = true
            });

            var result = validator.TestValidate(organization);
            result
                .ShouldHaveValidationErrorFor(organization => organization.Phones)
                .WithSeverity(Severity.Error)
                .WithErrorMessage("Can have only one Primary phone.");
        }


    }
}

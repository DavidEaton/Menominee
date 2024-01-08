using FluentAssertions;
using Menominee.Domain.Enums;
using Menominee.Domain.ValueObjects;
using System;
using TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class AddressShould
    {
        private const string AddressUnderMinimumLength = "11";
        private const string AddressOverMaximumLength = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla quis sem rutrum, mattis mi quis, euismod enim. Quisque fringilla pharetra tellus, nec vehicula libero ultricies sit amet. Sed pellentesque ornare consequat. Vestibulum sodales magna tempus egest"; // 257 characters
        private const string PostalCodeUnderMinimumLength = "4973";
        private const string PostalCodeOverMaximumLength = "497354973549735";

        [Fact]
        public void Create_Address()
        {
            var addressLine = "1234 Five Street";
            var addressLine2 = "Apt B";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode, addressLine2);

            addressOrError.Should().NotBeNull();
            addressOrError.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Should_Create_Address_Missing_AddressLine2()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.Should().NotBeNull();
            addressOrError.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_AddressLine()
        {
            string addressLine = null;
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.AddressRequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_AddressLine()
        {
            string addressLine = string.Empty;
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.AddressRequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_AddressLine_is_too_short()
        {
            string addressLine = "1";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.AddressLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_AddressLine_is_too_long()
        {
            string addressLine = Utilities.RandomCharacters(256);
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.AddressLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_AddressLine2_is_too_long()
        {
            var addressLine = "1234 Five Street";
            string addressLine2 = Utilities.RandomCharacters(256);
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode, addressLine2);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.AddressLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_City()
        {
            var addressLine = "1234 Five Street";
            string city = null;
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.CityRequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_City()
        {
            var addressLine = "1234 Five Street";
            string city = string.Empty;
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.CityRequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_City_is_too_short()
        {
            var addressLine = "1234 Five Street";
            string city = "c";
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.CityLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_City_is_too_long()
        {
            var addressLine = "1234 Five Street";
            string city = Utilities.RandomCharacters(256);
            var state = State.MI;
            var postalCode = "49735";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.CityLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_State_is_invalid()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = (State)111;
            string postalCode = "Z4566";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.StateInvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_PostalCode()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            string postalCode = null;

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.PostalCodeRequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_PostalCode()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            string postalCode = string.Empty;

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.PostalCodeRequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_PostalCode_is_too_short()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            string postalCode = "1";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.PostalCodeInvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_PostalCode_is_too_long()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            string postalCode = "12345678910";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.PostalCodeInvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_PostalCode_is_invalid()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            string postalCode = "Z4566";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);

            addressOrError.IsFailure.Should().BeTrue();
            addressOrError.Error.Should().Be(Address.PostalCodeInvalidMessage);
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var address1 = ContactableTestHelper.CreateAddress();
            var address2 = Address.Create(
                address1.AddressLine1, address1.City, address1.State, address1.PostalCode).Value;

            address1.Should().BeEquivalentTo(address2);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var address1 = ContactableTestHelper.CreateAddress();
            var address2 = ContactableTestHelper.CreateAddress();
            var newAddressLine = "54321";

            address2 = address2.NewAddressLine1(newAddressLine).Value;

            address1.Should().NotBeSameAs(address2);
        }

        [Fact]
        public void Return_New_Address_On_NewAddressLine1()
        {
            var address = ContactableTestHelper.CreateAddress();
            var newAddressLine = "5432 One Street";

            address = address.NewAddressLine1("5432 One Street").Value;

            address.AddressLine1.Should().Be(newAddressLine);
        }

        [Theory]
        [InlineData(AddressUnderMinimumLength)]
        [InlineData(AddressOverMaximumLength)]
        public void Return_Failure_On_NewAddressLine1_With_Invalid_Length(string addressLine)
        {
            var address = ContactableTestHelper.CreateAddress();

            var result = address.NewAddressLine1(addressLine);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("length");
        }

        [Fact]
        public void Throw_Exception_On_NewAddressLine1_Passing_Null_Parameter()
        {
            var address = ContactableTestHelper.CreateAddress();

            Action action = () => address = address.NewAddressLine1(null).Value;

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Return_New_Address_On_NewAddressLine2()
        {
            var address = ContactableTestHelper.CreateAddress();
            var newAddressLine2 = "Apt A";

            address = address.NewAddressLine2("Apt A").Value;

            address.AddressLine2.Should().Be(newAddressLine2);
        }

        [Theory]
        [InlineData(AddressOverMaximumLength)]
        public void Return_Failure_On_NewAddressLine2_With_Invalid_Length(string addressLine2)
        {
            var address = ContactableTestHelper.CreateAddress();

            var result = address.NewAddressLine2(addressLine2);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("length");
        }

        [Fact]
        public void Return_New_Address_On_NewCity()
        {
            var address = ContactableTestHelper.CreateAddress();
            var newCity = "Oomapopalis";

            address = address.NewCity(newCity).Value;

            address.City.Should().Be(newCity);
        }

        [Theory]
        [InlineData(AddressUnderMinimumLength)]
        [InlineData(AddressOverMaximumLength)]
        public void Return_Failure_On_Invalid_NewCity(string city)
        {
            var address = ContactableTestHelper.CreateAddress();

            var result = address.NewCity(city);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("length");
        }

        [Fact]
        public void Throw_Exception_On_NewCity_Passing_Null_Parameter()
        {
            var address = ContactableTestHelper.CreateAddress();

            Action action = () => address = address.NewCity(null).Value;

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Return_New_Address_On_NewState()
        {
            var address = ContactableTestHelper.CreateAddress();
            var newState = State.HI;
            address = address.NewState(newState).Value;

            address.State.Should().Be(newState);
        }

        [Fact]
        public void Return_Failure_On_NewState_With_Invalid_State()
        {
            var address = ContactableTestHelper.CreateAddress();
            var invalidState = (State)(-1);

            var result = address.NewState(invalidState);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Address.StateInvalidMessage);
        }

        [Fact]
        public void Return_New_Address_On_NewPostalCode()
        {
            var address = ContactableTestHelper.CreateAddress();
            var newPostalCode = "55555";

            address = address.NewPostalCode(newPostalCode).Value;

            address.PostalCode.Should().Be(newPostalCode);
        }

        [Theory]
        [InlineData(PostalCodeUnderMinimumLength)]
        [InlineData(PostalCodeOverMaximumLength)]
        public void Return_Failure_On_NewPostalCode_With_Invalid_PostalCode(string invalidPostalCode)
        {
            var address = ContactableTestHelper.CreateAddress();

            var result = address.NewPostalCode(invalidPostalCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Address.PostalCodeInvalidMessage);
        }

        [Fact]
        public void Throw_Exception_On_NewPostalCode_Passing_Null_Parameter()
        {
            var address = ContactableTestHelper.CreateAddress();

            Action action = () => address = address = address.NewPostalCode(null).Value;

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Return_ToString()
        {
            var address = ContactableTestHelper.CreateAddress();

            string toString = address.ToString();

            toString.Should().Be($"{address.AddressLine1} {address.City}, {address.State} {address.PostalCode}");
            toString.Should().Be(address.AddressFull);
        }
    }
}

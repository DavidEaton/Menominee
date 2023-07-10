using FluentAssertions;
using FluentAssertions.Execution;
using Menominee.Common.ValueObjects;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class PersonNameShould
    {
        private const string InvalidStringOverMaximumLength = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in"; // 256 characters
        private const string InvalidStringUnderMinimum = "";

        [Fact]
        public void Create_PersonName()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = PersonName.Create(lastName, firstName).Value;

            // Assert
            name.Should().NotBeNull();
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_Name()
        {
            var firstName = "";
            var lastName = "";

            var personNameOrError = PersonName.Create(firstName, lastName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_Name()
        {
            string firstName = null;
            string lastName = null;

            var personNameOrError = PersonName.Create(firstName, lastName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_LastName()
        {
            var firstName = "Molly";
            var lastName = "";

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Short_LastName()
        {
            var firstName = "Molly";
            var lastName = "";

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Long_LastName()
        {
            var firstName = "Molly";
            var lastName = Utilities.RandomCharacters(256);

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_FirstName()
        {
            var firstName = "";
            var lastName = "Moops";

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Theory]
        //[InlineData(InvalidStringZeroLength)]
        [InlineData(InvalidStringOverMaximumLength)]
        public void Return_IsFailure_Result_On_Create_With_Invalid_Length_FirstName(string firstName)
        {
            var lastName = "Moops";

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_LastName()
        {
            var firstName = "Molly";
            string lastName = null;

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_FirstName()
        {
            string firstName = null;
            var lastName = "Moops";

            var personNameOrError = PersonName.Create(lastName, firstName);

            personNameOrError.IsFailure.Should().BeTrue();
            personNameOrError.Error.Should().Be(PersonName.RequiredMessage);
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var middleInitial = "Z";

            var name1 = PersonName.Create(firstName, lastName, middleInitial).Value;
            var name2 = PersonName.Create(firstName, lastName, middleInitial).Value;

            name1.Should().Be(name2);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var newFirstName = "Jim";
            var newLastName = "Doane";

            var nameNew = PersonName.Create(newFirstName, newLastName).Value;

            name.Should().NotBe(nameNew);
        }

        [Fact]
        public void Return_New_PersonName_On_NewLastName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var newLastName = "Smith";

            name = name.NewLastName(newLastName).Value;

            name.LastName.Should().Be(newLastName);
        }

        [Theory]
        [InlineData(InvalidStringUnderMinimum)]
        [InlineData(InvalidStringOverMaximumLength)]
        public void Return_IsFailure_On_UnderMinimum_NewLastName(string newLastName)
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;

            var result = name.NewLastName(newLastName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PersonName.InvalidLengthMessage);
        }

        [Theory]
        [InlineData(InvalidStringOverMaximumLength)]
        public void Return_IsFailure_On_OverMaximumLength_NewLastName(string lastName)
        {
            var firstName = "Jane";

            var result = PersonName.Create(lastName, firstName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PersonName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_New_PersonName_On_NewFirstName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var newFirstName = "Smith";

            name = name.NewFirstName(newFirstName).Value;

            name.FirstName.Should().Be(newFirstName);
        }

        [Theory]
        [InlineData(InvalidStringUnderMinimum)]
        [InlineData(InvalidStringOverMaximumLength)]
        public void Return_IsFailure_On_UnderMinimum_NewFirstName(string newFirstName)
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;

            var result = name.NewFirstName(newFirstName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PersonName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_New_PersonName_On_NewMiddleName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var middleName = "Jingleheimer";
            var name = PersonName.Create(lastName, firstName, middleName).Value;
            var newMiddleName = "Allabaster";

            name = name.NewMiddleName(newMiddleName).Value;

            name.MiddleName.Should().Be(newMiddleName);
        }

        [Theory]
        [InlineData(InvalidStringOverMaximumLength)]
        public void Return_IsFailure_On_UnderMinimum_NewMiddleName(string newMiddleName)
        {
            var firstName = "Jane";
            var middleName = "Francis";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName, middleName).Value;

            var result = name.NewMiddleName(newMiddleName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PersonName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_ToString()
        {
            var firstName = "Jane";
            var middleName = "Francis";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName, middleName).Value;

            name.ToString().Should().Be(name.LastFirstMiddleInitial);
        }

        [Fact]
        public void Return_All_PersonName_Variants()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var middleName = "The";

            var name = PersonName.Create(lastName, firstName, middleName).Value;

            using (new AssertionScope())
            {
                name.FirstMiddleLast.Should().Be($"{firstName} {middleName} {lastName}");
                name.LastFirstMiddle.Should().Be($"{lastName}, {firstName} {middleName}");
                name.LastFirstMiddleInitial.Should().Be($"{lastName}, {firstName} {middleName[0]}.");
            }
        }
    }
}

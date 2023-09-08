using Bogus;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using Menominee.Domain.Entities;
using System;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class EmployeeShould
    {
        private readonly Faker faker = new();

        [Fact]
        public void Create_Employee()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public void Return_Failure_On_Create_EmployeeWith_Null_Person()
        {
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                null,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_Employee_With_Hired_Date_Earlier_Than_StartDateMinimum()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = Employee.StartDateMinimum.AddDays(-1); // Invalid hired date
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.DateRangeMessage);
        }

        [Fact]
        public void Create_Employee_With_Notes_Length_Greater_Than_NoteMaximumLength_Truncated()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(Employee.MaximumNoteLength + 1); // Invalid notes length
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public void Create_Employee_With_Null_RoleAssignments()
        {
            var person = new PersonFaker(true).Generate();
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                null,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Employee>();
        }

        [Fact]
        public void Return_Failure_On_SetHired_With_Date_Earlier_Than_StartDateMinimum()
        {
            var employee = new EmployeeFaker(true).Generate();
            var hired = Employee.StartDateMinimum.AddDays(-1); // Invalid hired date

            var result = employee.SetHired(hired);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.DateRangeMessage);
        }

        [Fact]
        public void SetHired()
        {
            var employee = new EmployeeFaker(true).Generate();
            var hired = DateTime.Today;

            var result = employee.SetHired(hired);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(hired);
        }

        [Fact]
        public void Return_Failure_On_SetExited_With_Date_Later_Than_EndDateMaximum()
        {
            var employee = new EmployeeFaker(true).Generate();
            var exited = Employee.EndDateMaximum.AddDays(1); // Invalid exited date

            var result = employee.SetExited(exited);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.DateRangeMessage);
        }

        [Fact]
        public void SetExited()
        {
            var employee = new EmployeeFaker(true).Generate();
            var exited = DateTime.Today;

            var result = employee.SetExited(exited);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(exited);
        }

        [Fact]
        public void SetNotes_With_Length_Greater_Than_NoteMaximumLength_Truncated()
        {
            var employee = new EmployeeFaker(true).Generate();
            var notes = faker.Random.String2(Employee.MaximumNoteLength + 1); // Invalid notes length

            var result = employee.SetNotes(notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(
                notes.Trim()
                .Truncate(Employee.MaximumNoteLength));
        }

        [Fact]
        public void SetNotes()
        {
            var employee = new EmployeeFaker(true).Generate();

            var notes = faker.Random.String2(1, Employee.MaximumNoteLength); // Valid notes length

            var result = employee.SetNotes(notes);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(notes);
        }
        [Fact]
        public void Return_Failure_On_AddRoleAssignment_With_Null_Assignment()
        {
            var employee = new EmployeeFaker(true).Generate();
            RoleAssignment assignment = null; // Null assignment

            var result = employee.AddRoleAssignment(assignment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.RequiredMessage);
        }

        [Fact]
        public void AddRoleAssignment()
        {
            var employee = new EmployeeFaker(true).Generate();
            var assignment = new RoleAssignmentFaker(true).Generate(); // Valid assignment

            var result = employee.AddRoleAssignment(assignment);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(assignment);
        }

        [Fact]
        public void Not_Create_Employee_With_Invalid_SSN()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var invalidSSN = Utilities.RandomCharacters(Employee.MaximumSSNLength + 1);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                invalidSSN,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidMaximumLengthMessage(Employee.MaximumSSNLength));
        }

        [Fact]
        public void Not_Create_Employee_With_Invalid_CertificationNumber()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var invalidCertificationNumber = Utilities.RandomCharacters(Employee.MaximumCertificationNumberLength + 1);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                invalidCertificationNumber,
                active,
                printedName,
                expenseCategory,
                benefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidMaximumLengthMessage(Employee.MaximumCertificationNumberLength));
        }

        [Fact]
        public void Not_Create_Employee_With_Invalid_PrintedName()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var invalidPrintedName = Utilities.RandomCharacters(Employee.MaximumPrintedNameLength + 1);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                invalidPrintedName,
                expenseCategory,
                benefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidMaximumLengthMessage(Employee.MaximumPrintedNameLength));
        }

        [Fact]
        public void Not_Create_Employee_With_Invalid_ExpenseCategory()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var invalidExpenseCategory = (EmployeeExpenseCategory)(-1);
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                invalidExpenseCategory,
                benefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidExpenseCategoryMessage);
        }

        [Fact]
        public void Not_Create_Employee_With_Invalid_BenefitLoad()
        {
            var person = new PersonFaker(true).Generate();
            var roleAssignments = new RoleAssignmentFaker(true).Generate(3);
            var hired = DateTime.Today;
            var notes = faker.Random.String2(1, 500);
            var ssn = faker.Random.String2(1, 12);
            var certificationNumber = faker.Random.String2(1, 20);
            var active = faker.Random.Bool();
            var printedName = faker.Random.String2(1, 50);
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();
            var invalidBenefitLoad = Employee.MaximumBenefitLoad + 1;

            var result = Employee.Create(
                person,
                roleAssignments,
                hired,
                notes,
                ssn,
                certificationNumber,
                active,
                printedName,
                expenseCategory,
                invalidBenefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.BenefitLoadMessage);
        }

        [Fact]
        public void SetSSN()
        {
            var employee = new EmployeeFaker(true).Generate();
            var ssn = faker.Random.String2(1, 12);

            var result = employee.SetSSN(ssn);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(ssn);
        }

        [Fact]
        public void SetCertificationNumber()
        {
            var employee = new EmployeeFaker(true).Generate();
            var certificationNumber = faker.Random.String2(1, 20);

            var result = employee.SetCertificationNumber(certificationNumber);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(certificationNumber);
        }

        [Fact]
        public void SetActive()
        {
            var employee = new EmployeeFaker(true).Generate();
            var active = faker.Random.Bool();

            var result = employee.SetActive(active);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(active);
        }

        [Fact]
        public void SetPrintedName()
        {
            var employee = new EmployeeFaker(true).Generate();
            var printedName = faker.Random.String2(1, 50);

            var result = employee.SetPrintedName(printedName);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(printedName);
        }



        [Fact]
        public void SetExpenseCategory()
        {
            var employee = new EmployeeFaker(true).Generate();
            var expenseCategory = faker.Random.Enum<EmployeeExpenseCategory>();

            var result = employee.SetExpenseCategory(expenseCategory);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(expenseCategory);
        }

        [Fact]
        public void SetBenefitLoad()
        {
            var employee = new EmployeeFaker(true).Generate();
            var benefitLoad = faker.Random.Double(0.0, 100.0);

            var result = employee.SetBenefitLoad(benefitLoad);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(benefitLoad);
        }

        [Fact]
        public void Not_SetSSN_With_Invalid_SSN()
        {
            var employee = new EmployeeFaker(true).Generate();
            var invalidSSN = Utilities.RandomCharacters(Employee.MaximumSSNLength + 1);

            var result = employee.SetSSN(invalidSSN);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidMaximumLengthMessage(Employee.MaximumSSNLength));
        }

        [Fact]
        public void Not_SetCertificationNumber_With_Invalid_CertificationNumber()
        {
            var employee = new EmployeeFaker(true).Generate();
            var invalidCertificationNumber = Utilities.RandomCharacters(Employee.MaximumCertificationNumberLength + 1);

            var result = employee.SetCertificationNumber(invalidCertificationNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidMaximumLengthMessage(Employee.MaximumCertificationNumberLength));
        }

        [Fact]
        public void Not_SetPrintedName_With_Invalid_PrintedName()
        {
            var employee = new EmployeeFaker(true).Generate();
            var invalidPrintedName = Utilities.RandomCharacters(Employee.MaximumPrintedNameLength + 1);

            var result = employee.SetPrintedName(invalidPrintedName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidMaximumLengthMessage(Employee.MaximumPrintedNameLength));
        }

        [Fact]
        public void Not_SetExpenseCategory_With_Invalid_ExpenseCategory()
        {
            var employee = new EmployeeFaker(true).Generate();
            var invalidExpenseCategory = (EmployeeExpenseCategory)(-1);

            var result = employee.SetExpenseCategory(invalidExpenseCategory);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.InvalidExpenseCategoryMessage);
        }

        [Fact]
        public void Not_SetBenefitLoad_With_Invalid_BenefitLoad()
        {
            var employee = new EmployeeFaker(true).Generate();
            var invalidBenefitLoad = Employee.MaximumBenefitLoad + 1;

            var result = employee.SetBenefitLoad(invalidBenefitLoad);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Employee.BenefitLoadMessage);
        }
    }
}

using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities;

public class Employee : Entity
{
    public static readonly DateTime StartDateMinimum = DateTime.Today.AddYears(-50);
    public static readonly DateTime EndDateMaximum = DateTime.Today.AddYears(1);
    public static readonly int MaximumNoteLength = 10000;
    public static readonly int MaximumSSNLength = 12;
    public static readonly int MaximumCertificationNumberLength = 20;
    public static readonly int MaximumPrintedNameLength = 50;
    public static readonly double MinimumBenefitLoad = 0.0;
    public static readonly double MaximumBenefitLoad = 100.0;

    public static readonly string RequiredMessage = $"Please include all required items.";
    public static readonly string DateRangeMessage = $"Employment date(s) invalid.";
    public static readonly string InvalidExpenseCategoryMessage = $"Expense category is invalid.";
    public static readonly string BenefitLoadMessage = $"Benefit load must be between {MinimumBenefitLoad} and {MaximumBenefitLoad}";
    public static string InvalidMaximumLengthMessage(int max) => $"Value must be less than {max} characters in length";

    public Person PersonalDetails { get; private set; }
    public DateTime? Hired { get; private set; } = null;
    public DateTime? Exited { get; private set; }
    public string CompanyEmployeeId { get; set; }
    public IReadOnlyList<RoleAssignment> RoleAssignments => roleAssignments.ToList();
    private readonly List<RoleAssignment> roleAssignments = new();
    public string Notes { get; private set; }
    public string SSN { get; private set; }
    public string CertificationNumber { get; private set; }
    public bool Active { get; private set; } = true;
    public string PrintedName { get; private set; }
    public EmployeeExpenseCategory ExpenseCategory { get; private set; } = EmployeeExpenseCategory.CostOfDirectLabor;
    public double BenefitLoad { get; private set; } = 0.0;

    private Employee(Person personalDetails, List<RoleAssignment> roleAssignments, DateTime? hired, string notes, string ssn, string certificationNumber, bool active, string printedName, EmployeeExpenseCategory expenseCategory, double benefitLoad)
    {
        PersonalDetails = personalDetails;
        Hired = hired;
        Notes = notes;
        SSN = ssn;
        CertificationNumber = certificationNumber;
        Active = active;
        PrintedName = printedName;
        ExpenseCategory = expenseCategory;
        BenefitLoad = benefitLoad;

        if (roleAssignments is not null)
            foreach (var assignment in roleAssignments)
                AddRoleAssignment(assignment);
    }

    public Result<RoleAssignment> AddRoleAssignment(RoleAssignment assignment)
    {
        if (assignment is null)
            return Result.Failure<RoleAssignment>(RequiredMessage);

        roleAssignments.Add(assignment);

        return Result.Success(assignment);
    }

    public static Result<Employee> Create(Person personalDetails, List<RoleAssignment> roleAssignments, DateTime? hired = null, string notes = null, string ssn = null, string certificationNumber = null, bool active = true, string printedName = null, EmployeeExpenseCategory expenseCategory = EmployeeExpenseCategory.CostOfDirectLabor, double benefitLoad = 0.0)
    {
        if (personalDetails is null)
            return Result.Failure<Employee>(RequiredMessage);

        if (hired.HasValue)
            if (hired < StartDateMinimum)
                return Result.Failure<Employee>(DateRangeMessage);

        notes = (notes ?? string.Empty).Trim().Truncate(MaximumNoteLength);

        if (!string.IsNullOrWhiteSpace(notes) && notes.Length > MaximumNoteLength)
            return Result.Failure<Employee>(InvalidMaximumLengthMessage(MaximumNoteLength));

        ssn = (ssn ?? string.Empty).Trim();
        var ssnResult = ValidateSSN(ssn);
        if (ssnResult.IsFailure)
            return Result.Failure<Employee>(ssnResult.Error);

        certificationNumber = (certificationNumber ?? string.Empty).Trim();
        var certificationNumberResult = ValidateCertificationNumber(certificationNumber);
        if (certificationNumberResult.IsFailure)
            return Result.Failure<Employee>(certificationNumberResult.Error);

        printedName = (printedName ?? string.Empty).Trim();
        var printedNameResult = ValidatePrintedName(printedName);
        if (printedNameResult.IsFailure)
            return Result.Failure<Employee>(printedNameResult.Error);

        var expenseCategoryResult = ValidateExpenseCategory(expenseCategory);
        if (expenseCategoryResult.IsFailure)
            return Result.Failure<Employee>(expenseCategoryResult.Error);

        var benefitLoadResult = ValidateBenefitLoad(benefitLoad);
        if (benefitLoadResult.IsFailure)
            return Result.Failure<Employee>(benefitLoadResult.Error);

        return Result.Success(new Employee(personalDetails, roleAssignments, hired, notes, ssn, certificationNumber, active, printedName, expenseCategory, benefitLoad));
    }

    private static Result ValidateSSN(string ssn)
    {
        if (string.IsNullOrWhiteSpace(ssn)) return Result.Success();

        return ssn.Length <= MaximumSSNLength
            ? Result.Success()
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumSSNLength));
    }

    private static Result ValidateCertificationNumber(string certificationNumber)
    {
        if (string.IsNullOrWhiteSpace(certificationNumber)) return Result.Success();

        return certificationNumber.Length <= MaximumCertificationNumberLength
            ? Result.Success()
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumCertificationNumberLength));
    }

    private static Result ValidatePrintedName(string printedName)
    {
        if (string.IsNullOrWhiteSpace(printedName)) return Result.Success();

        return printedName.Length <= MaximumPrintedNameLength
            ? Result.Success()
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumPrintedNameLength));
    }

    private static Result ValidateExpenseCategory(EmployeeExpenseCategory expenseCategory)
    {
        return Enum.IsDefined(typeof(EmployeeExpenseCategory), expenseCategory)
            ? Result.Success()
            : Result.Failure<EmployeeExpenseCategory>(InvalidExpenseCategoryMessage);
    }

    private static Result ValidateBenefitLoad(double benefitLoad)
    {
        return benefitLoad >= MinimumBenefitLoad && benefitLoad <= MaximumBenefitLoad
            ? Result.Success()
            : Result.Failure<double>(BenefitLoadMessage);
    }

    public Result<DateTime> SetHired(DateTime hired)
    {
        return hired < StartDateMinimum
            ? Result.Failure<DateTime>(DateRangeMessage)
            : Result.Success((DateTime)(Hired = hired));
    }

    public Result<DateTime> SetExited(DateTime exited)
    {
        return exited > EndDateMaximum
            ? Result.Failure<DateTime>(DateRangeMessage)
            : Result.Success((DateTime)(Hired = exited));
    }

    public Result<string> SetNotes(string notes)
    {
        return Result.Success(Notes = notes.Trim().Truncate(MaximumNoteLength));
    }

    public Result<string> SetSSN(string ssn)
    {
        if (string.IsNullOrWhiteSpace(ssn))
        {
            return Result.Success(SSN = string.Empty);
        }

        return ssn.Length <= MaximumSSNLength
            ? Result.Success(SSN = ssn)
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumSSNLength));
    }

    public Result<string> SetCertificationNumber(string certificationNumber)
    {
        if (string.IsNullOrWhiteSpace(certificationNumber))
        {
            return Result.Success(CertificationNumber = string.Empty);
        }

        return certificationNumber.Length <= MaximumCertificationNumberLength
            ? Result.Success(CertificationNumber = certificationNumber)
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumCertificationNumberLength));
    }

    public Result<bool> SetActive(bool active = true)
    {
        return Result.Success(Active = active);
    }

    public Result<string> SetPrintedName(string printedName)
    {
        if (string.IsNullOrWhiteSpace(printedName))
        {
            return Result.Success(PrintedName = string.Empty);
        }

        return printedName.Length <= MaximumPrintedNameLength
            ? Result.Success(PrintedName = printedName)
            : Result.Failure<string>(InvalidMaximumLengthMessage(MaximumPrintedNameLength));
    }

    public Result<EmployeeExpenseCategory> SetExpenseCategory(EmployeeExpenseCategory expenseCategory)
    {
        return Enum.IsDefined(typeof(EmployeeExpenseCategory), expenseCategory)
            ? Result.Success(ExpenseCategory = expenseCategory)
            : Result.Failure<EmployeeExpenseCategory>(InvalidExpenseCategoryMessage);
    }

    public Result<double> SetBenefitLoad(double benefitLoad)
    {
        return benefitLoad >= MinimumBenefitLoad && benefitLoad <= MaximumBenefitLoad
            ? Result.Success(BenefitLoad = benefitLoad)
            : Result.Failure<double>(BenefitLoadMessage);
    }

    #region ORM

    // EF requires a parameterless constructor
    protected Employee()
    {
        roleAssignments = new List<RoleAssignment>();
    }

    #endregion
}

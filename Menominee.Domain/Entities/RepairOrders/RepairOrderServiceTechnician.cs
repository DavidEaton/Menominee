using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    public class RepairOrderServiceTechnician : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";

        public Employee Employee { get; private set; }

        private RepairOrderServiceTechnician(Employee employee)
        {
            Employee = employee;
        }

        public static Result<RepairOrderServiceTechnician> Create(Employee employee)
        {
            return employee is null
                ? Result.Failure<RepairOrderServiceTechnician>(RequiredMessage)
                : Result.Success(new RepairOrderServiceTechnician(employee));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderServiceTechnician() { }

        #endregion
    }
}
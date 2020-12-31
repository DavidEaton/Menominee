using Migrations.Core.Enums;
using SharedKernel;
using System;

namespace Migrations.Core.Entities
{
    public class EmployeeRole : Entity
    {
        public Employee Employee { get; set; }
        public EmploymentRole EmploymentRole { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidThru { get; set; }
        public void EndRole(DateTime roleEnded)
        {
            SetEndRole(roleEnded);
        }

        public DateTime? GetRoleEnded()
        {
            return ValidThru;
        }

        private void SetEndRole(DateTime? roleEnded)
        {
            ValidThru = roleEnded;
        }

        public bool Active { get => !GetRoleEnded().HasValue; }
    }
}

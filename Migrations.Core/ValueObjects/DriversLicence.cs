using SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace Migrations.Core.ValueObjects
{
    public class DriversLicence : ValueObject<DriversLicence>
    {
        [Required]
        public string Number { get; private set; }
        //[Required]
        //public DateTimeRange ValidFromThru { get; set; }

        [Required]
        public string State { get; private set; }

        public DriversLicence(string number, string state)
        {
            Number = number;
            State = state;
        }

        protected DriversLicence() { }

        public DriversLicence NewNumber(string newNumber)
        {
            return new DriversLicence(newNumber, State);
        }
        public DriversLicence NewState(string newState)
        {
            return new DriversLicence(Number, newState);
        }
    }
}

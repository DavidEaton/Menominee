using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Vehicle : Entity
    {
        public Vehicle(string vin, int year, string make, string model, Customer customer)
        {
            VIN = vin;
            Year = year;
            Make = make;
            Model = model;
            Customer = customer;
        }
        public string VIN { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public virtual Customer Customer { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        protected Vehicle() { }

        #endregion


    }
}

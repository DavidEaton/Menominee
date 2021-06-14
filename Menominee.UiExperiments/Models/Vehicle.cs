namespace Menominee.UiExperiments.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Customer Customer { get; set; }

        #region ORM

        // EF requires an empty constructor
        protected Vehicle() { }

        #endregion


    }
}

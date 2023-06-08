namespace CustomerVehicleManagement.Shared.Models.Vehicles
{
    public class VehicleToRead
    {
        public long Id { get; set; }
        public string VIN { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public override string ToString()
        {
            return $"{Year} {Make} {Model}";
        }
    }
}

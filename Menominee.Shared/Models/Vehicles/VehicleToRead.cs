using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Vehicles
{
    public class VehicleToRead
    {
        public long Id { get; set; }
        public string VIN { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public State? PlateStateProvince { get; set; }
        public string UnitNumber { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; } = true;
        public bool NonTraditionalVehicle { get; set; } = false;

        public override string ToString()
        {
            return $"{Year} {Make} {Model}";
        }
    }
}

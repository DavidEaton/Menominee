namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class EmailToWrite
    {
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}
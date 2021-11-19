namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailToAdd
    {
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}

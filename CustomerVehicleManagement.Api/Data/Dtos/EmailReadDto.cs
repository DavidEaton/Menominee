namespace CustomerVehicleManagement.Api.Data.Dtos
{
    public class EmailReadDto
    {
        //public int Id { get; set; } EMAIL IS NOT AN AGGREGATE ROOT
        public string Address { get; set; }
        public bool IsPrimary { get; set; }

    }
}

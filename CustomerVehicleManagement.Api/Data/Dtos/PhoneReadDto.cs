namespace CustomerVehicleManagement.Api.Data.Dtos
{
    public class PhoneReadDto
    {
        //public int Id { get; set; } PHONE IS NOT AN AGGREGATE ROOT
        public string Number { get; set; }
        public string PhoneType { get; set; }
        public bool IsPrimary { get; set; }

    }
}

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class PersonListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => $"{AddressLine} {City}, {State}  {PostalCode}"; }
        public string Phone { get; set; }
        public string PhoneType { get; set; }


    }
}

namespace Menominee.Shared.Models.Businesses
{
    public class BusinessToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public string Notes { get; set; }
        public string ContactName { get; set; }
        public string ContactPrimaryPhone { get; set; }
    }
}

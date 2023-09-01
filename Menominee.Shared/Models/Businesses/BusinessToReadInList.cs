namespace Menominee.Shared.Models.Businesses
{
    public class BusinessToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull
        {
            get => string.IsNullOrWhiteSpace(AddressLine1)
                ? $"{string.Empty}"
                : string.IsNullOrWhiteSpace(AddressLine2)
                ? $"{AddressLine1}, {City}, {State} {PostalCode}"
                : $"{AddressLine1}, {AddressLine2}, {City}, {State} {PostalCode}";
        }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public string Notes { get; set; }
        public string ContactName { get; set; }
        public string ContactPrimaryPhone { get; set; }
    }
}

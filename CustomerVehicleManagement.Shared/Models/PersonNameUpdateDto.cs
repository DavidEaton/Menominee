namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonNameUpdateDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }

        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName[0]}.";
        }
        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }

        public override string ToString()
        {
            return LastFirstMiddleInitial;
        }
    }
}

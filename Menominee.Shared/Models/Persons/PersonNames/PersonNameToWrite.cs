namespace Menominee.Shared.Models.Persons
{
    public class PersonNameToWrite
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(MiddleName)
                ? $"{LastName}, {FirstName}"
                : $"{LastName}, {FirstName} {MiddleName}";
        }

        public static implicit operator string(PersonNameToWrite personNameToWrite)
        {
            return personNameToWrite?.ToString() ?? string.Empty;
        }
    }
}

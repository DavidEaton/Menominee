namespace Menominee.Shared.Models.Businesses
{
    public class BusinessNameRequest
    {
        public string Name { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

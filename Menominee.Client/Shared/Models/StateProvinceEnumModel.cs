using Menominee.Domain.Enums;

namespace Menominee.Client.Shared.Models
{
    public class StateProvinceEnumModel
    {
        public State Value { get; set; }
        public string? DisplayText { get; set; }
        public string? FullDisplayText { get; set; }
    }
}

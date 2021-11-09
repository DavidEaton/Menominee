using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class RegisterUserResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}

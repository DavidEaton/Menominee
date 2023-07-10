using System.Collections.Generic;

namespace Menominee.Shared.Models
{
    public class RegisterUserResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}

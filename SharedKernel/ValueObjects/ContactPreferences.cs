using SharedKernel.Utilities;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class ContactPreferences : ValueObject
    {
        public bool AllowMail { get; private set; }
        public bool AllowEmail { get; private set; }
        public bool AllowSms { get; private set; }

        private ContactPreferences(bool allowMail, bool allowEmail, bool allowSms)
        {
            AllowMail = allowMail;
            AllowEmail = allowEmail;
            AllowSms = allowSms;
        }

        public static Result<ContactPreferences> Create()
        {
            return Result.Ok(new ContactPreferences(false, false, false));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AllowEmail;
            yield return AllowEmail;
            yield return AllowSms;
        }
    }
}

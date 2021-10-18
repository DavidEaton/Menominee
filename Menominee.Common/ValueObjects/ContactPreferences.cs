using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class ContactPreferences : AppValueObject
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

        public static Result<ContactPreferences> Create(bool allowMail, bool allowEmail, bool allowSms)
        {
            return Result.Success(new ContactPreferences(allowMail, allowEmail, allowSms));
        }

        public ContactPreferences NewAllowMail(bool allowMail)
        {
            return new ContactPreferences(allowMail, AllowEmail, AllowSms);
        }

        public ContactPreferences NewAllowEmail(bool allowEmail)
        {
            return new ContactPreferences(AllowMail, allowEmail, AllowSms);
        }

        public ContactPreferences NewAllowSms(bool allowSms)
        {
            return new ContactPreferences(AllowMail, AllowEmail, allowSms);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AllowEmail;
            yield return AllowEmail;
            yield return AllowSms;
        }
    }
}

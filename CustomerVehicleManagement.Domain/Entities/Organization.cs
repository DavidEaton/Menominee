using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Contactable
    {
        public static readonly int NoteMaximumLength = 10000;
        public static readonly string NoteMaximumLengthMessage = $"Organization note cannot be over {NoteMaximumLength} characters in length.";

        public Organization(OrganizationName name)
        {
            Name = name;
        }

        public OrganizationName Name { get; private set; }
        public virtual Person Contact { get; private set; }
        public string Note { get; private set; }

        public void SetName(OrganizationName name)
        {
            Name = name;
        }

        public void SetContact(Person contact)
        {
            if (contact != null)
                Contact = contact;
        }

        public Result<string> SetNote(string note)
        {
            if (string.IsNullOrWhiteSpace(note))
                return Result.Failure<string>("Can't add an empty note");

            note = note.Trim();

            if (note.Length <= NoteMaximumLength)
                return Result.Failure<string>(NoteMaximumLengthMessage);

            Note = note;
            return Result.Success(Note);
        }

        #region ORM

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}

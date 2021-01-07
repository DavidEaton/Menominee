using Migrations.Core.ValueObjects;
using SharedKernel;

namespace Migrations.Core.Entities
{
    internal class PhoneOfPerson : Entity
    {
        /// <summary>
        /// This entity will act as a thin wrapper on top of Phone value object
        /// with just an identifier and a reference to the owning Person.
        /// Necessary to enable searchable value object collection of phones.
        /// Although we do now have an additional entity in our domain model,
        /// we don’t ever have to expose it outside of the aggregate (Person).
        /// </summary>
        public Phone Phone { get; private set; }
        public Person Person { get; private set; }

        public PhoneOfPerson(Phone phone, Person person)
        {
            Person = person;
            Phone = phone;
        }
    }
}

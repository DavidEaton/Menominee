using CustomerVehicleManagement.Api.Data;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.IntegrationTests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Persons.AddRange(GetSeedingPersons());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            db.Persons.RemoveRange(db.Persons);
            InitializeDbForTests(db);
        }

        public static List<Person> GetSeedingPersons()
        {
            return new List<Person>()
            {
                new Person(new PersonName("Smith", "Jane"), Gender.Female),
                new Person(new PersonName("Jones", "Latisha"), Gender.Female),
                new Person(new PersonName("Lee", "Wong"), Gender.Male),
                new Person(new PersonName("Kelly", "Junice"), Gender.Female),
            };
        }
    }
}

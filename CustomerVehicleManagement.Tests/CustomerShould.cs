using CustomerVehicleManagement.Core.Model;
using NUnit.Framework;

namespace CustomerVehicleManagement.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateNewCustomer()
        {
            var person = new Person
            {
                FirstName = "Jane",
                LastName = "Doe"
            };

            var customer = new Customer(person);

            
            Person jane = (Person)customer.Entity;

            Assert.That("Doe, Jane", Is.EqualTo(jane.NameLastFirst));
        }

    }
}
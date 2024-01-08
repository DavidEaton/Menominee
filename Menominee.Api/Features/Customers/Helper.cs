using Menominee.Api.Data;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Customers
{
    public class Helper
    {
        public static async Task LoadRelatedDataAsync(Customer customer, ApplicationDbContext context)
        {
            await LoadCustomerEntity(customer, context);
            await LoadVehiclesAsync(customer, context);
            await LoadContactDetailsAsync(customer, context);
        }

        public static async Task LoadCustomerEntity(Customer customer, ApplicationDbContext context)
        {
            var personId = context.Entry(customer).Property("PersonId").CurrentValue;
            var businessId = context.Entry(customer).Property("BusinessId").CurrentValue;

            if (personId is not null)
            {
                var person = await context.Persons.FindAsync(personId);
                if (person is null)
                {
                    return;
                }
                var result = customer.SetCustomerEntity(person);
                if (result.IsFailure)
                {
                    return;
                }
            }

            if (businessId is not null)
            {
                var business = await context.Businesses.FindAsync(businessId);
                if (business is null)
                {
                    return;
                }
                var result = customer.SetCustomerEntity(business);
                if (result.IsFailure)
                {
                    return;
                }
            }
        }

        internal static async Task LoadVehiclesAsync(Customer customer, ApplicationDbContext context)
        {
            await context.Entry(customer)
                .Collection(c => c.Vehicles)
                .LoadAsync();
        }

        public static void SetShadowProperties(DbContext context, Customer customer)
        {
            if (customer is null) return;

            var customerEntry = context.Attach(customer).Entity;

            if (customer.CustomerEntity is Entity entity)
            {

                // Explicitly set the shadow properties
                if (GetEntityType(customer.CustomerEntity) == EntityType.Business)
                {
                    context.Entry(customerEntry).Property("BusinessId").CurrentValue = entity.Id;
                    var business = customer.CustomerEntity as Business;

                    if (business?.Contact is not null)
                    {
                        _ = context.Attach(business.Contact).Entity;
                    }
                }

                if (GetEntityType(customer.CustomerEntity) == EntityType.Person)
                {
                    context.Entry(customerEntry).Property("PersonId").CurrentValue = entity.Id;
                }
            }

            else
            {
                Log.Error("Unknown customer entity type.");
            }
        }

        private static EntityType GetEntityType(ICustomerEntity entity) => entity switch
        {
            Person => EntityType.Person,
            Business => EntityType.Business,
            _ => throw new InvalidOperationException("Unknown entity type")
        };

        internal static async Task LoadContactDetailsAsync(Customer customer, ApplicationDbContext context)
        {
            if (customer.CustomerEntity is Entity entity)
            {

                if (GetEntityType(customer.CustomerEntity) == EntityType.Business)
                {
                    var business = customer.CustomerEntity as Business;

                    await context.Entry(business)
                        .Collection(b => b.Phones)
                        .LoadAsync();

                    await context.Entry(business)
                        .Collection(b => b.Emails)
                        .LoadAsync();

                    if (business?.Contact is not null)
                    {
                        //_ = context.Attach(business.Contact).Entity;

                        //await context.Entry(business)
                        //    .Reference(b => b.Contact)
                        //    .LoadAsync();
                    }
                }

                if (GetEntityType(customer.CustomerEntity) == EntityType.Person)
                {
                    var person = customer.CustomerEntity as Person;

                    await context.Entry(person)
                        .Collection(p => p.Phones)
                        .LoadAsync();

                    await context.Entry(person)
                        .Collection(p => p.Emails)
                        .LoadAsync();
                }
            }
        }
    }
}

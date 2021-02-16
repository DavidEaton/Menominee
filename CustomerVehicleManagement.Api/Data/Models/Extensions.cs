using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public static class Extensions
    {
        public static CustomerReadDto ToReadDto(this Customer customer)
        {
            if (customer != null)
            {
                if (customer.EntityType == EntityType.Organization)
                {
                    Organization organization = customer.Entity as Organization;

                    return new CustomerReadDto
                    {
                        Id = customer.Id,
                        EntityType = EntityType.Organization,
                        Name = organization.Name,
                        Address = organization.Address,
                        CustomerType = customer.CustomerType,
                        AllowMail = customer.AllowMail,
                        AllowEmail = customer.AllowEmail,
                        AllowSms = customer.AllowSms,
                        PriceProfileId = customer.PriceProfileId,
                        TaxIds = customer.TaxIds,
                        RewardsMember = customer.RewardsMember,
                        OverrideCustomerTaxProfile = customer.OverrideCustomerTaxProfile,


                        Contact = (organization.Contact != null)
                        ? new PersonReadDto()
                        {
                            Id = organization.Contact.Id,
                            Address = organization.Contact.Address,
                            Gender = organization.Contact.Gender,
                            Name = organization.Contact.Name.LastFirstMiddle
                            //Phones = (IList<PhoneReadDto>)organization.Phones
                        }
                        : null
                    };
                }

                if (customer.EntityType == EntityType.Person)
                {
                    Person person = customer.Entity as Person;

                    return new CustomerReadDto
                    {
                        Id = customer.Id,
                        Name = person.Name.LastFirstMiddle,
                        Address = person.Address,
                    };
                }
            }

            return null;
        }
    }
}

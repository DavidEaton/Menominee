using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Helpers;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerToReadInList
    {
        public long Id { get; set; }
        public EntityType EntityType { get; set; }
        public long EntityId { get; set; }
        public string Name { get; set; }
        public string AddressFull { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public string CustomerType { get; set; }
        public string Note { get; set; }
        public string ContactName { get; set; }
        public string ContactPrimaryPhone { get; set; }
        public string ContactPrimaryPhoneType { get; set; }

        public static CustomerToReadInList ConvertToDto(Customer customer)
        {
            if (customer != null)
            {
                return new CustomerToReadInList()
                {
                    Id = customer.Id,
                    Name = customer.EntityType == EntityType.Organization
                                                        ? customer.Organization.Name.Name
                                                        : customer.Person.Name.LastFirstMiddle,
                    EntityId = customer.EntityType == EntityType.Organization
                                                        ? customer.Organization.Id
                                                        : customer.Person.Id,
                    EntityType = customer.EntityType,
                    CustomerType = customer.CustomerType.ToString(),
                    AddressFull = customer.EntityType == EntityType.Organization
                                                        ? customer.Organization?.Address?.AddressFull
                                                        : customer.Person?.Address?.AddressFull,
                    Note = customer.EntityType == EntityType.Organization
                                                        ? customer.Organization?.Note
                                                        : string.Empty,
                    PrimaryPhone = customer.EntityType == EntityType.Organization
                                                        ? PhoneHelper.GetPrimaryPhone(customer?.Organization)
                                                        : PhoneHelper.GetPrimaryPhone(customer?.Person),
                    PrimaryPhoneType = customer.EntityType == EntityType.Organization
                                                        ? PhoneHelper.GetPrimaryPhoneType(customer?.Organization)
                                                        : PhoneHelper.GetPrimaryPhoneType(customer?.Person),
                    PrimaryEmail = customer.EntityType == EntityType.Organization
                                                        ? EmailHelper.GetPrimaryEmail(customer?.Organization)
                                                        : EmailHelper.GetPrimaryEmail(customer?.Person)
                    //ContactName = customer.EntityType == EntityType.Organization
                    //                                    ? customer?.Organization?.Contact.Name.LastFirstMiddle
                    //                                    : string.Empty,
                    //ContactPrimaryPhone = customer.EntityType == EntityType.Organization
                    //                                    ? PhoneHelper.GetPrimaryPhone(customer?.Organization?.Contact)
                    //                                    : string.Empty,
                    //ContactPrimaryPhoneType = customer.EntityType == EntityType.Organization
                    //                                    ? PhoneHelper.GetPrimaryPhoneType(customer?.Organization?.Contact)
                    //                                    : string.Empty
                };
            }

            return null;
        }
    }
}

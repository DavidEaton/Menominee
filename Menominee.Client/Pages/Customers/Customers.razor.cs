using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Customers
{
    public partial class Customers : ComponentBase
    {
        [Inject]
        public ICustomerDataService CustomerDataService { get; set; }

        public IReadOnlyList<CustomerToReadInList> CustomersList;
        List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();

        private CustomerToWrite CustomerToWrite { get; set; }
        List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();

        private long Id { get; set; }
        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;
        private string Caption { get; set; }
        protected override async Task OnInitializedAsync()
        {
            CustomersList = (await CustomerDataService.GetAllCustomers()).ToList();

            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = item.ToString(), Value = item });

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as CustomerToReadInList).Id;
            Editing = true;
            Caption = "Editing Customer";
            CustomersList = null;

            var readDto = await CustomerDataService.GetCustomer(Id);
            CustomerToWrite = new CustomerToWrite
            {
                EntityType = readDto.EntityType,
                CustomerType = readDto.CustomerType
            };

            if (readDto.EntityType == EntityType.Person)
            {
                CustomerToWrite.Person = new()
                {
                    Name = new()
                    {
                        LastName = readDto.Person?.LastName,
                        MiddleName = readDto.Person?.MiddleName,
                        FirstName = readDto.Person?.FirstName
                    },

                    Gender = readDto.Person.Gender,
                    Birthday = readDto.Person?.Birthday
                };

                if (readDto.Person.Address is not null)
                    CustomerToWrite.Person.Address = new()
                    {
                        AddressLine = readDto.Person.Address?.AddressLine,
                        City = readDto.Person.Address?.City,
                        State = readDto.Person.Address.State,
                        PostalCode = readDto.Person.Address?.PostalCode
                    };

                if (readDto.Person.DriversLicense is not null)
                    CustomerToWrite.Person.DriversLicense = new()
                    {
                        Number = readDto.Person.DriversLicense?.Number,
                        State = readDto.Person.DriversLicense.State,
                        Issued = readDto.Person.DriversLicense.Issued,
                        Expiry = readDto.Person.DriversLicense.Expiry
                    };

                if (readDto.Person.Phones.Count > 0)
                {
                    foreach (var phone in readDto.Person.Phones)
                    {
                        CustomerToWrite.Person.Phones.Add(new()
                        {
                            Number = phone.Number,
                            PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                            IsPrimary = phone.IsPrimary
                        });
                    }
                }

                if (readDto.Person.Emails.Count > 0)
                {
                    foreach (var email in readDto.Person.Emails)
                    {
                        CustomerToWrite.Person.Emails.Add(new()
                        {
                            Address = email.Address,
                            IsPrimary = email.IsPrimary
                        });
                    }
                }
            }

            if (readDto.EntityType == EntityType.Organization)
            {
                CustomerToWrite.Organization = new()
                {
                    Name = readDto.Organization.Name,
                    Note = readDto.Organization.Note,

                    Address = new()
                    {
                        AddressLine = readDto.Organization.Address.AddressLine,
                        City = readDto.Organization.Address.City,
                        State = readDto.Organization.Address.State,
                        PostalCode = readDto.Organization.Address.PostalCode
                    }
                };

                if (readDto.Organization.Contact != null)
                {
                    CustomerToWrite.Organization.Contact = new()
                    {
                        Name = new()
                        {
                            LastName = readDto.Organization.Contact.LastName,
                            MiddleName = readDto.Organization.Contact.MiddleName,
                            FirstName = readDto.Organization.Contact.FirstName
                        },

                        Gender = readDto.Organization.Contact.Gender,
                        Birthday = readDto.Organization.Contact?.Birthday

                    };

                    if (readDto.Organization.Contact.Address is not null)
                        CustomerToWrite.Organization.Contact.Address = new()
                        {
                            AddressLine = readDto.Organization.Contact.Address.AddressLine,
                            City = readDto.Organization.Contact.Address.City,
                            State = readDto.Organization.Contact.Address.State,
                            PostalCode = readDto.Organization.Contact.Address.PostalCode
                        };


                    if (readDto.Organization.Contact.DriversLicense is not null)
                        CustomerToWrite.Organization.Contact.DriversLicense = new()
                        {
                            Number = CustomerToWrite.Organization.Contact.DriversLicense.Number,
                            State = CustomerToWrite.Organization.Contact.DriversLicense.State,
                            Issued = CustomerToWrite.Organization.Contact.DriversLicense.Issued,
                            Expiry = CustomerToWrite.Organization.Contact.DriversLicense.Expiry
                        };

                    if (CustomerToWrite.Organization.Contact.Phones.Count > 0)
                    {
                        foreach (var phone in CustomerToWrite.Organization.Contact.Phones)
                        {
                            CustomerToWrite.Person.Phones.Add(new()
                            {
                                Number = phone.Number,
                                PhoneType = phone.PhoneType,
                                IsPrimary = phone.IsPrimary
                            });
                        }
                    }

                    if (CustomerToWrite.Organization.Contact.Emails.Count > 0)
                    {
                        foreach (var email in CustomerToWrite.Organization.Contact.Emails)
                        {
                            CustomerToWrite.Person.Emails.Add(new()
                            {
                                Address = email.Address,
                                IsPrimary = email.IsPrimary
                            });
                        }
                    }
                }
            }
        }

        private void Add()
        {
            Adding = true;
            Caption = "Adding new Customer";
            CustomersList = null;
            CustomerToWrite = new()
            {
                CustomerType = CustomerType.Retail,
                EntityType = EntityType.Person,
                Person = new()
            };
        }

        private void EntityTypeChanged()
        {
            if (CustomerToWrite.EntityType == EntityType.Organization)
            {
                if (CustomerToWrite.Organization is null)
                    CustomerToWrite.Organization = new();

                CustomerToWrite.Person = null;
            }

            if (CustomerToWrite.EntityType == EntityType.Person)
            {
                if (CustomerToWrite.Person is null)
                    CustomerToWrite.Person = new();

                CustomerToWrite.Organization = null;
            }
        }

        //private void EntityTypeChanged(EntityType entityType)
        //{
        //    CustomerToWrite.EntityType = entityType;

        //    if (entityType == EntityType.Person)
        //    {
        //        if (CustomerToWrite.Person is null)
        //            CustomerToWrite.Person = new();

        //        CustomerToWrite.Organization = null;
        //    }

        //    if (entityType == EntityType.Organization)
        //    {
        //        if (CustomerToWrite.Organization is null)
        //            CustomerToWrite.Organization = new();

        //        CustomerToWrite.Person = null;
        //    }
        //}

        protected async Task AddSubmit()
        {
            await CustomerDataService.AddCustomer(CustomerToWrite);
            await Close();
        }

        protected async Task EditSubmit()
        {
            await CustomerDataService.UpdateCustomer(CustomerToWrite, Id);
            await EndEditAsync();
        }

        protected async Task Submit()
        {
            if (Adding)
                await AddSubmit();

            if (Editing)
                await EditSubmit();
        }

        protected async Task EndEditAsync()
        {
            Editing = false;
            Caption = string.Empty;
            CustomersList = (await CustomerDataService.GetAllCustomers()).ToList();
        }

        protected async Task Close()
        {
            CustomerToWrite = null;
            Adding = false;
            Editing = false;
            Caption = string.Empty;
            CustomersList = (await CustomerDataService.GetAllCustomers()).ToList();
        }

        private class CustomerTypeEnumModel
        {
            public CustomerType Value { get; set; }
            public string DisplayText { get; set; }
        }

        private class EntityTypeEnumModel
        {
            public EntityType Value { get; set; }
            public string DisplayText { get; set; }
        }

    }
}

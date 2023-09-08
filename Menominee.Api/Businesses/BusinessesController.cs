using Menominee.Api.Common;
using Menominee.Api.Persons;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Api.Businesses
{
    public class BusinessesController : BaseApplicationController<BusinessesController>
    {
        private readonly IBusinessRepository repository;
        private readonly IPersonRepository personsRepository;
        private readonly PersonsController personsController;
        private readonly string BasePath = "/api/businesses";

        public BusinessesController(
            IBusinessRepository repository
            , PersonsController personsController
            , IPersonRepository personsRepository
            , ILogger<BusinessesController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            this.personsController = personsController ??
                throw new ArgumentNullException(nameof(personsController));
            this.personsRepository = personsRepository;
        }

        // api/businesses/list
        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<BusinessToReadInList>>> GetBusinessesListAsync()
        {
            var businesses = await repository.GetBusinessesListAsync();

            if (businesses is null)
                return NotFound();

            return Ok(businesses);
        }

        // api/businesses
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BusinessToRead>>> GetBusinessesAsync()
        {
            var businesses = await repository.GetBusinessesAsync();

            if (businesses is null)
                return NotFound();

            return Ok(businesses);
        }

        // api/businesses/1
        [HttpGet("{id:long}", Name = "GetBusinessAsync")]
        public async Task<ActionResult<BusinessToRead>> GetBusinessAsync(long id)
        {
            var business = await repository.GetBusinessAsync(id);

            if (business is null)
                return NotFound();

            return business;
        }

        // api/businesses/1
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateBusinessAsync(long id, BusinessToWrite businessFromCaller)
        {
            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Save changes
                4) return NoContent()
            */

            var notFoundMessage = $"Could not find Business to update: {businessFromCaller.Name}";

            if (!await repository.BusinessExistsAsync(id))
                return NotFound(notFoundMessage);

            //1) Get domain entity from repository
            var businessFromRepository = await repository.GetBusinessEntityAsync(id);

            if (businessFromRepository is null)
                return NotFound(notFoundMessage);

            // 2) Update domain entity with data in data transfer object(DTO)
            if (businessFromRepository.Name.Name != businessFromCaller.Name)
            {
                var businessNameResult = businessFromRepository.Name.NewBusinessName(businessFromCaller.Name);

                if (businessNameResult.IsFailure)
                    return BadRequest(businessNameResult.Error);

                businessFromRepository.SetName(businessNameResult.Value);
            }

            if (businessFromCaller?.Address is not null)
            {
                var addressResult = Address.Create(
                        businessFromCaller.Address.AddressLine1,
                        businessFromCaller.Address.City,
                        businessFromCaller.Address.State,
                        businessFromCaller.Address.PostalCode,
                        businessFromCaller.Address.AddressLine2);

                if (addressResult.IsFailure)
                    return BadRequest(addressResult.Error);

                businessFromRepository.SetAddress(addressResult.Value);
            }

            // Client may send an empty or null Address VALUE OBJECT, signifying REMOVAL
            if (businessFromCaller?.Address is null)
                businessFromRepository.ClearAddress();

            businessFromRepository.SetNotes(businessFromCaller.Notes);

            // Client may send an empty or null phones collection of ENTITIES, signifying REMOVAL
            foreach (var phone in businessFromCaller?.Phones)
            {
                if (phone.Id == 0)
                    businessFromRepository.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (phone.Id != 0)
                {
                    var contextPhone = businessFromRepository.Phones.FirstOrDefault(contextPhone => contextPhone.Id == phone.Id);
                    contextPhone.SetNumber(phone.Number);
                    contextPhone.SetIsPrimary(phone.IsPrimary);
                    contextPhone.SetPhoneType(phone.PhoneType);
                }

                if (phone.Id != 0)
                    businessFromRepository.RemovePhone(
                        businessFromRepository.Phones.FirstOrDefault(
                            contextPhone =>
                            contextPhone.Id == phone.Id));
            }

            // Client may send an empty or null emails collection, signifying REMOVAL
            List<Email> emails = new();
            if (businessFromCaller?.Emails.Count > 0)
            {
                emails.AddRange(businessFromCaller.Emails
                    .Select(email =>
                            Email.Create(email.Address,
                                         email.IsPrimary).Value));
            }

            var contactDetails = ContactDetailsFactory.Create(
                businessFromCaller.Phones, businessFromCaller.Emails, businessFromCaller.Address).Value;

            businessFromRepository.UpdateContactDetails(contactDetails);

            businessFromRepository.SetNotes(businessFromCaller.Notes);

            // Contact
            if (businessFromCaller?.Contact is not null && businessFromCaller.Contact.IsNotEmpty)
            {
                var result = await personsController.UpdatePersonAsync(
                                    businessFromRepository.Contact.Id,
                                    businessFromCaller.Contact);

                var person = await personsRepository.GetPersonEntityAsync(businessFromRepository.Contact.Id);

                if (person != null)
                    businessFromRepository.SetContact(person);
            }

            /* Returning the updated resource is acceptible, for example:
                 return Ok(businessFromRepository);
               even preferred over returning NoContent if updated resource
               contains properties that are mutated by the data store
               (which they are not in this case).

               Instead, our app will:
                 return NoContent();
               ... and let the caller decide whether to get the updated resource,
               which is also acceptible. The HTTP specification (RFC 2616) has a
               number of recommendations that are applicable:
            HTTP status code 200 OK for a successful PUT of an update to an existing resource. No response body needed.
            HTTP status code 201 Created for a successful PUT of a new resource
            HTTP status code 409 Conflict for a PUT that is unsuccessful due to a 3rd-party modification
            HTTP status code 400 Bad Request for an unsuccessful PUT
            */

            await repository.SaveChangesAsync();

            return NoContent();
        }

        private static void UpdateAddress(BusinessToWrite businessFromCaller, Business businessFromRepository)
        {
            if (businessFromCaller?.Address != null)
                businessFromRepository.SetAddress(
                    Address.Create(
                        businessFromCaller.Address.AddressLine1,
                        businessFromCaller.Address.City,
                        businessFromCaller.Address.State,
                        businessFromCaller.Address.PostalCode,
                        businessFromCaller.Address.AddressLine2).Value);

            // Client may send an empty or null Address VALUE OBJECT, signifying REMOVAL
            if (businessFromCaller?.Address is null)
                businessFromRepository.SetAddress(null);
        }

        [HttpPost]
        public async Task<IActionResult> AddBusinessAsync(BusinessToWrite businessToAdd)
        {
            /*
                Web API controllers don't have to check ModelState.IsValid if they have the
                [ApiController] attribute; most of our controllers inherit from ApplicationController,
                which has the [ApiController] attribute. With [ApiController] attribute applied,
                an automatic HTTP 400 response containing error details is returned when model
                state is invalid.*/

            /* Controller Pattern:
                1. Convert data contract/data transfer object (dto) to domain entity
                2. Add domain entity to repository
                3. Save changes on repository
                4. Return to consumer */

            // 1. Convert dto to domain entity
            var business = BusinessHelper.ConvertWriteDtoToEntity(businessToAdd);

            // 2. Add domain entity to repository
            await repository.AddBusinessAsync(business);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new id and route to new resource after save
            return Created(new Uri($"{BasePath}/{business.Id}",
                               UriKind.Relative),
                               new { business.Id });
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteBusinessAsync(long id)
        {
            /* Delete Pattern in Controllers:
             1) Get domain entity from repository
             2) Call repository.Delete(), which removes entity from context
             3) Save changes
             4) return NoContent()
            */
            var businessFromRepository = await repository.GetBusinessEntityAsync(id);

            if (businessFromRepository is null)
                return NotFound($"Could not find Business in the database to delete with Id: {id}.");

            repository.DeleteBusiness(businessFromRepository);

            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
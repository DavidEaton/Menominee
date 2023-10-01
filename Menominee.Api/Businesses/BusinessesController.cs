using Menominee.Api.Common;
using Menominee.Api.Persons;
using Menominee.Common.Http;
using Menominee.Shared.Models.Businesses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Businesses
{
    public class BusinessesController : BaseApplicationController<BusinessesController>
    {
        private readonly IBusinessRepository repository;
        private readonly IPersonRepository personsRepository;
        private readonly PersonsController personsController;
        // This is the only production code file that I prefer to keep comments in. - DE

        // TODO: Dependency Injection: Avoid injecting one controller into another
        // (PersonsController into BusinessesController). This tightly couples the
        // controllers and may lead to issues down the line. Use domain services or
        // application services for shared functionality instead.
        public BusinessesController(
            IBusinessRepository repository,
             PersonsController personsController,
             IPersonRepository personsRepository,
             ILogger<BusinessesController> logger) : base(logger)
        {
            this.repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            this.personsController = personsController ??
                throw new ArgumentNullException(nameof(personsController));
            this.personsRepository = personsRepository ??
                throw new ArgumentNullException(nameof(personsRepository));
        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<BusinessToReadInList>>> GetListAsync()
        {
            var businesses = await repository.GetListAsync();

            if (businesses is null)
                return NotFound();

            return Ok(businesses);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BusinessToRead>>> GetAllAsync()
        {
            var result = await repository.GetAllAsync();

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<BusinessToRead>> GetAsync(long id)
        {
            var result = await repository.GetAsync(id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateAsync(BusinessToWrite businessFromCaller)
        {
            /* Update Pattern in Controllers:
                1) Get domain entity from repository
                2) Update domain entity with data in data transfer object (DTO)
                3) Save changes
                4) return NoContent()
            */

            //1) Get domain entity from repository
            var businessFromRepository = await repository.GetEntityAsync(businessFromCaller.Id);

            if (businessFromRepository is null)
                return NotFound($"Could not find Business to update: {businessFromCaller.Name}");

            // 2) Update domain entity with data in data transfer object(DTO)
            Updaters.UpdateBusiness(businessFromCaller, businessFromRepository);

            // 3) Save changes to wherever the repository is pointing
            await repository.SaveChangesAsync();

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

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> AddAsync(BusinessToWrite businessToAdd)
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
            // No need to validate it here again, just call .Value right away
            var business = BusinessHelper.ConvertWriteDtoToEntity(businessToAdd);

            // 2. Add domain entity to repository
            repository.Add(business);

            // 3. Save changes on repository
            await repository.SaveChangesAsync();

            // 4. Return new id and route to new resource after save
            return Created(new Uri($"api/businesses/{business.Id}",
                UriKind.Relative),
                new { business.Id });
        }


        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            /* Delete Pattern in Controllers:
             1) Get domain entity from repository
             2) Call repository.Delete(), which removes entity from context
             3) Save changes
             4) return NoContent()
            */
            var businessFromRepository = await repository.GetEntityAsync(id);

            if (businessFromRepository is null)
                return NotFound($"Could not find Business in the database to delete with Id: {id}.");

            repository.Delete(businessFromRepository);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}